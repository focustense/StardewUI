using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Default in-game view engine.
/// </summary>
/// <param name="viewFactory">Factory for creating views, based on their tag names.</param>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the types required
/// by the target view or structural property.</param>
/// <param name="viewBinder">Binding service used to create <see cref="IViewBinding"/> instances that detect changes to
/// data or assets and propagate them to the bound <see cref="IView"/>.</param>
/// <param name="assetCache">Cache for obtaining document assets. Used for included views.</param>
/// <param name="resolutionScopeFactory">Factory for creating <see cref="IResolutionScope"/> instances responsible for
/// resolving external symbols such as translation keys.</param>
public class ViewNodeFactory(
    IViewFactory viewFactory,
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IViewBinder viewBinder,
    IAssetCache assetCache,
    IResolutionScopeFactory resolutionScopeFactory
) : IViewNodeFactory
{
    /// <inheritdoc />
    public IViewNode CreateNode(Document document)
    {
        var scope = resolutionScopeFactory.CreateForDocument(document);
        return CreateNode(document.Root, scope);
    }

    /// <inheritdoc />
    public IViewNode CreateNode(SNode node, IResolutionScope resolutionScope)
    {
        return CreateNodeChild(node, null, resolutionScope).Node;
    }

    record SwitchContext(IViewNode Node, IAttribute Attribute, IResolutionScope ResolutionScope);

    private IViewNode.Child CreateNodeChild(SNode node, SwitchContext? switchContext, IResolutionScope resolutionScope)
    {
        using var _ = Trace.Begin(this, nameof(CreateNode));
        var (innerNode, outletName) = CreateNodeChildWithoutBackoff(node, switchContext, resolutionScope);
        var outerNode = new BackoffNodeDecorator(innerNode, BackoffRule.Default);
        return new(outerNode, outletName);
    }

    private IViewNode.Child CreateNodeChildWithoutBackoff(
        SNode node,
        SwitchContext? switchContext,
        IResolutionScope resolutionScope
    )
    {
        var structuralAttributes = StructuralAttributes.Get(node.Attributes);
        if (structuralAttributes.Repeat is IAttribute repeatAttr)
        {
            return new(new RepeaterNode(valueSourceFactory, CreateNonRepeatingNodeChild, resolutionScope, repeatAttr));
        }
        else
        {
            return CreateNonRepeatingNodeChild();
        }

        IViewNode CreateDefaultViewNode()
        {
            return new ViewNode(
                valueSourceFactory,
                viewFactory,
                viewBinder,
                node.Element,
                resolutionScope,
                contextAttribute: structuralAttributes.Context
            );
        }

        IViewNode CreateIncludedViewNode()
        {
            var assetNameAttribute = node.Attributes.SingleOrDefault(attr =>
                attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase)
            );
            if (assetNameAttribute is null)
            {
                throw new BindingException("<include> node is missing a 'name' attribute.", node);
            }
            return new IncludedViewNode(
                valueSourceFactory,
                valueConverterFactory,
                assetCache,
                resolutionScope,
                doc => CreateNodeChild(doc.Root, switchContext, resolutionScopeFactory.CreateForDocument(doc)).Node,
                assetNameAttribute,
                structuralAttributes.Context
            );
        }

        IViewNode.Child CreateNonRepeatingNodeChild()
        {
            var viewNode = node.Tag.Equals("include", StringComparison.OrdinalIgnoreCase)
                ? CreateIncludedViewNode()
                : CreateDefaultViewNode();
            IViewNode result = viewNode;
            if (structuralAttributes.Case is IAttribute caseAttr)
            {
                if (switchContext is null)
                {
                    throw new BindingException(
                        $"Cannot bind *case attribute (value: '{caseAttr.Value}') without a prior *switch.",
                        node
                    );
                }
                var condition = new BinaryCondition(
                    valueSourceFactory,
                    valueConverterFactory,
                    switchContext.ResolutionScope,
                    switchContext.Attribute,
                    resolutionScope,
                    caseAttr
                )
                {
                    LeftContextSelector = () => switchContext.Node.Context,
                };
                result = new ConditionalNode(viewNode, condition);
            }
            if (structuralAttributes.If is not null)
            {
                var condition = new UnaryCondition(
                    valueSourceFactory,
                    valueConverterFactory,
                    resolutionScope,
                    structuralAttributes.If
                );
                result = new ConditionalNode(result, condition);
            }
            var nextSwitchContext = structuralAttributes.Switch is IAttribute switchAttr
                ? new SwitchContext(viewNode, switchAttr, resolutionScope)
                : switchContext;
            if (viewNode is ViewNode defaultViewNode)
            {
                defaultViewNode.Children = node
                    .ChildNodes.Select(n => CreateNodeChild(n, nextSwitchContext, resolutionScope))
                    .ToList();
            }
            return new(result, structuralAttributes.Outlet?.Value);
        }
    }

    class StructuralAttributes
    {
        public IAttribute? Case { get; set; }
        public IAttribute? Context { get; set; }
        public IAttribute? If { get; set; }
        public IAttribute? Outlet { get; set; }
        public IAttribute? Repeat { get; set; }
        public IAttribute? Switch { get; set; }

        public static StructuralAttributes Get(IReadOnlyList<IAttribute> attributes)
        {
            var result = new StructuralAttributes();
            foreach (var attribute in attributes)
            {
                switch (attribute.Name)
                {
                    case "case":
                        result.Case = attribute;
                        break;
                    case "context":
                        result.Context = attribute;
                        break;
                    case "if":
                        result.If = attribute;
                        break;
                    case "outlet":
                        result.Outlet = attribute;
                        break;
                    case "repeat":
                        result.Repeat = attribute;
                        break;
                    case "switch":
                        result.Switch = attribute;
                        break;
                }
            }
            return result;
        }
    }
}
