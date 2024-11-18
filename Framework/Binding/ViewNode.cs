using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using StardewUI.Framework.Content;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Internal structure of a view node, encapsulating dependencies required for data binding and lazy creation/updates.
/// </summary>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="viewFactory">Factory for creating views, based on their tag names.</param>
/// <param name="viewBinder">Binding service used to create <see cref="IViewBinding"/> instances that detect changes to
/// data or assets and propagate them to the bound <see cref="IView"/>.</param>
/// <param name="element">Element data for this node.</param>
/// <param name="resolutionScope">Scope for resolving externalized attributes, such as translation keys.</param>
/// <param name="contextAttribute">Optional attribute specifying how to resolve the context for child nodes based on
/// this node's assigned <see cref="Context"/>.</param>
public class ViewNode(
    IValueSourceFactory valueSourceFactory,
    IViewFactory viewFactory,
    IViewBinder viewBinder,
    SElement element,
    IResolutionScope resolutionScope,
    IAttribute? contextAttribute = null
) : IViewNode
{
    /// <inheritdoc />
    public IReadOnlyList<IViewNode.Child> Children
    {
        get => children;
        set
        {
            children = value;
            childNodesByOutlet = children
                .GroupBy(c => c.OutletName ?? "", c => c.Node)
                .ToDictionary(g => g.Key, g => g.ToList() as IReadOnlyList<IViewNode>);
        }
    }

    /// <inheritdoc />
    public BindingContext? Context
    {
        get => context;
        set
        {
            if (value == context)
            {
                return;
            }
            context = value;
            wasContextChanged = true;
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IView> Views => view is not null ? [view] : [];

    /// <summary>
    /// Pre-initializes some reflection state in order to make future invocations faster.
    /// </summary>
    /// <remarks>
    /// This method uses reflection and should only be invoked during background startup.
    /// </remarks>
    /// <typeparam name="TView">The view type.</typeparam>
    internal static void Warmup<TView>()
        where TView : IView
    {
        ReflectionChildrenBinder.Warmup<TView>();
    }

    private IViewBinding? binding;
    private IValueSource? childContextSource;
    private IReadOnlyList<IViewNode.Child> children = [];
    private Dictionary<string, IReadOnlyList<IViewNode>> childNodesByOutlet = [];
    private IChildrenBinder? childrenBinder;
    private BindingContext? context;
    private IView? view;
    private bool wasContextChanged;

    /// <inheritdoc />
    public void Dispose()
    {
        Reset();
        if (childContextSource is IDisposable childContextDisposable)
        {
            childContextDisposable.Dispose();
        }
        childContextSource = null;
        context = null;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Print(StringBuilder sb, bool includeChildren)
    {
        IElement printableElement = element;
        if (includeChildren && Children.Count > 0)
        {
            printableElement.Print(sb);
            foreach (var child in Children)
            {
                child.Node.Print(sb, includeChildren);
            }
            printableElement.PrintClosingTag(sb);
        }
        else
        {
            printableElement.Print(sb, true);
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        foreach (var child in Children)
        {
            child.Node.Reset();
        }
        binding?.Dispose();
        binding = null;
        childrenBinder = null;
        view = null;
        wasContextChanged = false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();
        Print(sb, true);
        return sb.ToString();
    }

    /// <inheritdoc />
    public bool Update(TimeSpan elapsed)
    {
        using var _ = Trace.Begin(this, nameof(Update));
        bool wasChanged = false;
        if (view is null)
        {
            view ??= viewFactory.CreateView(element.Tag);
            var viewDescriptor = viewBinder.GetDescriptor(view);
            childrenBinder = ReflectionChildrenBinder.FromViewDescriptor(viewDescriptor);
        }
        bool wasChildContextChanged = false;
        if (wasContextChanged)
        {
            if (contextAttribute is not null)
            {
                var childContextType = valueSourceFactory.GetValueType(contextAttribute, null, context);
                childContextSource = childContextType is not null
                    ? valueSourceFactory.GetValueSource(childContextType, contextAttribute, context, resolutionScope)
                    : null;
            }
            else
            {
                childContextSource = context?.Data is not null ? new ConstantValueSource<object?>(context.Data) : null;
            }
            wasChanged = true;
            wasChildContextChanged = true;
            binding?.Dispose();
            binding = null;
        }
        if (binding is not null)
        {
            wasChanged |= binding.Update();
        }
        else
        {
            // Don't require explicit update because IViewBinder.Bind always does an initial forced update.
            binding = viewBinder.Bind(view, element, context, resolutionScope);
            wasChanged = true;
        }
        if (childContextSource is not null)
        {
            wasChildContextChanged |= childContextSource.Update();
            wasChanged |= wasChildContextChanged;
        }
        bool wasChildViewChanged = false;
        foreach (var childNode in Children.Select(c => c.Node))
        {
            if (wasChildContextChanged)
            {
                childNode.Context = childContextSource?.Value is not null
                    ? contextAttribute is not null
                        ? BindingContext.Create(childContextSource.Value, Context)
                        : Context
                    : null;
            }
            // Even though Views is an IReadOnlyList<IView>, that does not make it an immutable list. If we want to
            // reliably detect changes, we have to account for the possibility of the list being modified in situ.
            var previousViews = new List<IView>(childNode.Views);
            wasChanged |= childNode.Update(elapsed);
            wasChildViewChanged |= !childNode.Views.SequenceEqual(previousViews);
        }
        if (wasChildViewChanged)
        {
            UpdateViewChildren();
            wasChanged = true;
        }
        wasContextChanged = false;
        return wasChanged;
    }

    private void UpdateViewChildren()
    {
        using var _ = Trace.Begin(this, nameof(UpdateViewChildren));
        if (view is null)
        {
            return;
        }
        var children = Children
            .SelectMany(child => child.Node.Views)
            .Where(view => view is not null)
            .Cast<IView>()
            .ToList();
        if (childrenBinder is not null)
        {
            foreach (var (outletName, childNodes) in childNodesByOutlet)
            {
                var childViews = childNodes
                    .SelectMany(node => node.Views)
                    .Where(view => view is not null)
                    .Cast<IView>();
                childrenBinder.SetChildren(view, outletName, childViews);
            }
        }
        else if (children.Count > 0)
        {
            throw new BindingException(
                $"Cannot bind {children.Count} children to view type {view.GetType().Name} because it does not "
                    + "define any publicly writable child/children property."
            );
        }
    }

    interface IChildrenBinder
    {
        void SetChildren(IView view, string? outletName, IEnumerable<IView> children);
    }

    static class ReflectionChildrenBinder
    {
        private static readonly ConcurrentDictionary<Type, IChildrenBinder?> cache = [];
        private static readonly MethodInfo factoryMethodDefinition = typeof(ReflectionChildrenBinder).GetMethod(
            nameof(CreateChildrenBinder),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

        public static IChildrenBinder? FromViewDescriptor(IViewDescriptor viewDescriptor)
        {
            using var _ = Trace.Begin(nameof(ReflectionChildrenBinder), nameof(FromViewDescriptor));
            return cache.GetOrAdd(
                viewDescriptor.TargetType,
                _ =>
                {
                    var factoryMethod = factoryMethodDefinition.MakeGenericMethod(viewDescriptor.TargetType);
                    return (IChildrenBinder)factoryMethod.Invoke(null, [viewDescriptor])!;
                }
            );
        }

        /// <summary>
        /// Pre-initializes some reflection state in order to make future invocations faster.
        /// </summary>
        internal static void Warmup<TView>()
        {
            FromViewDescriptor(DescriptorFactory.GetViewDescriptor(typeof(TView)));
        }

        private static IChildrenBinder CreateChildrenBinder<TView>(IViewDescriptor viewDescriptor)
            where TView : IView
        {
            return new ReflectionChildrenBinder<TView>(viewDescriptor);
        }
    }

    class ReflectionChildrenBinder<TView>(IViewDescriptor viewDescriptor) : IChildrenBinder
        where TView : IView
    {
        private static readonly ConcurrentDictionary<string, IOutletBinder?> outletCache =
            new(StringComparer.InvariantCultureIgnoreCase);
        private static readonly MethodInfo multipleMethod = typeof(ReflectionChildrenBinder<TView>).GetMethod(
            nameof(Multiple),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;
        private static readonly MethodInfo singleMethod = typeof(ReflectionChildrenBinder<TView>).GetMethod(
            nameof(Single),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

        internal static void Warmup()
        {
            multipleMethod.MakeGenericMethod(typeof(View), typeof(IView));
            singleMethod.MakeGenericMethod(typeof(View), typeof(IView));
        }

        public void SetChildren(IView view, string? outletName, IEnumerable<IView> children)
        {
            var outletBinder = outletCache.GetOrAdd(
                outletName ?? "",
                _ =>
                {
                    var childrenProperty = viewDescriptor.GetChildrenProperty(outletName);
                    return childrenProperty is not null
                        ? CreateOutletBinder(
                            !string.IsNullOrEmpty(outletName) ? outletName : defaultOutletName,
                            childrenProperty
                        )
                        : null;
                }
            );
            if (outletBinder is not null)
            {
                outletBinder.SetChildren(view, children);
            }
            else
            {
                throw new BindingException(
                    $"View type {viewDescriptor.TargetType.Name} does not have an outlet named '{outletName}."
                );
            }
        }

        private static IOutletBinder CreateOutletBinder(string outletName, IPropertyDescriptor childrenProperty)
        {
            using var _ = Trace.Begin(nameof(ReflectionChildrenBinder<TView>), nameof(CreateOutletBinder));
            var enumerableChildType = childrenProperty.ValueType.GetEnumerableElementType();
            var factoryMethod = enumerableChildType is not null
                ? multipleMethod.MakeGenericMethod(enumerableChildType, childrenProperty.ValueType)
                : singleMethod.MakeGenericMethod(childrenProperty.ValueType);
            return (IOutletBinder)factoryMethod.Invoke(null, [outletName, childrenProperty])!;
        }

        private static readonly string defaultOutletName = "(default)";

        private static OutletBinder<TView, TChildren> Multiple<TChild, TChildren>(
            string outletName,
            IPropertyDescriptor<TChildren> property
        )
            where TChildren : IEnumerable<TChild>
        {
            return new((view, children) => property.SetValue(view, children!), true, outletName);
        }

        private static OutletBinder<TView, TChild> Single<TChild>(
            string outletName,
            IPropertyDescriptor<TChild?> property
        )
            where TChild : notnull
        {
            return new((view, child) => property.SetValue(view, child), false, outletName);
        }
    }

    interface IOutletBinder
    {
        void SetChildren(IView view, IEnumerable<IView> children);
    }

    class OutletBinder<TView, TChildren>(Action<TView, TChildren?> setChildren, bool allowsMultiple, string outletName)
        : IOutletBinder
        where TView : IView
    {
        public void SetChildren(IView view, IEnumerable<IView> children)
        {
            using var _ = Trace.Begin(this, nameof(SetChildren));
            if (allowsMultiple)
            {
                setChildren((TView)view, (TChildren)(object)children.ToList());
            }
            else
            {
                var firstChildren = children.Take(2).ToArray();
                if (firstChildren.Length > 1)
                {
                    throw new BindingException(
                        $"Cannot bind multiple children to outlet {outletName} of view type {typeof(TView).Name} "
                            + "because it only supports a single child/content view."
                    );
                }
                var child = firstChildren.Length > 0 ? firstChildren[0] : null;
                setChildren((TView)view, (TChildren?)(object?)child);
            }
        }
    }
}
