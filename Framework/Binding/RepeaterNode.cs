using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A structural node that accepts a collection (<see cref="IEnumerable{T}"/>) valued attribute and repeats its inner
/// elements with each child bound to a collection element, in the same order as the collection.
/// </summary>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="innerNodeCreator">Delegate for creating a new child node.</param>
/// <param name="repeatAttribute">The attribute containing the collection expression.</param>
public class RepeaterNode(
    IValueSourceFactory valueSourceFactory,
    Func<IViewNode> innerNodeCreator,
    IAttribute repeatAttribute
) : IViewNode
{
    public IReadOnlyList<IViewNode> ChildNodes => childNodes;

    public BindingContext? Context
    {
        get => context;
        set
        {
            if (value != context)
            {
                context = value;
                wasContextChanged = true;
            }
        }
    }

    public IReadOnlyList<IView> Views { get; private set; } = [];

    private List<IViewNode> childNodes = [];
    private ICollectionWatcher? collectionWatcher;
    private BindingContext? context;
    private bool wasContextChanged;

    public void Dispose()
    {
        Reset();
        context = null;
        wasContextChanged = false;
        GC.SuppressFinalize(this);
    }

    public void Reset()
    {
        foreach (var childNode in childNodes)
        {
            childNode.Dispose();
        }
        childNodes = [];
        collectionWatcher?.Dispose();
        collectionWatcher = null;
        // Ensure that if the node gets reused (Updated again) then we recreate the collection watcher etc., even if the
        // context hasn't changed again.
        // This is the opposite of what gets done in e.g. ViewNode, but is intentionally done this way here because
        // those implementations will automatically recreate any missing state, whereas RepeaterNode doesn't because it
        // is expensive to run those checks on every frame when the context didn't change.
        wasContextChanged = true;
    }

    public bool Update()
    {
        if (wasContextChanged)
        {
            var collectionType = valueSourceFactory.GetValueType(repeatAttribute, null, context);
            var collectionSource = collectionType is not null
                ? valueSourceFactory.GetValueSource(repeatAttribute, context, collectionType)
                : null;
            collectionWatcher?.Dispose();
            collectionWatcher = collectionSource is not null ? CollectionWatcher.Create(collectionSource) : null;
            if (collectionWatcher is null)
            {
                // This means (generally) that the node is unbound, which is OK, unlike having an incompatible type.
                Reset();
                // Important to update wasContextChanged after the reset, as it will set wasContextChanged = true.
                wasContextChanged = false;
                return true;
            }
            wasContextChanged = false;
        }
        bool result = UpdateChildBindings();
        // Even if the "tree" itself wasn't updated, we still have to pass the update down to existing child nodes.
        foreach (var childNode in childNodes)
        {
            result |= childNode.Update();
        }
        if (result)
        {
            Views = childNodes.SelectMany(node => node.Views).ToList();
        }
        return result;
    }

    private void AddChildNodes(int fromIndex, IList? items)
    {
        if (items is null)
        {
            return;
        }
        var newNodes = new List<IViewNode>(items.Count);
        foreach (var item in items)
        {
            var newNode = innerNodeCreator();
            newNode.Context = item is not null ? BindingContext.Create(item) : null;
            // Operating on assumption that repeatedly appending to a temporary list and then inserting the whole list
            // is faster than inserting items one by one (and repeatedly shifting any subsequent items) into the middle.
            newNodes.Add(newNode);
        }
        childNodes.InsertRange(fromIndex, newNodes);
    }

    private void MoveChildNodes(int fromIndex, int toIndex, int count)
    {
        var oldItems = childNodes.GetRange(fromIndex, count);
        childNodes.RemoveRange(fromIndex, count);
        childNodes.InsertRange(toIndex, oldItems);
    }

    private void ReplaceChildNodes(int fromIndex, IList? oldItems, IList? newItems)
    {
        var count = oldItems?.Count ?? newItems?.Count ?? 0;
        if (count == 0)
        {
            return;
        }
        for (int i = 0; i < count; i++)
        {
            var newItem = newItems?.Count > i ? newItems[i] : null;
            childNodes[fromIndex + i].Context = newItem is not null ? BindingContext.Create(newItem) : null;
        }
    }

    private bool UpdateChildBindings()
    {
        // Node was already unbound (or couldn't be resolved), and is still unbound, so nothing to do here.
        if (collectionWatcher is null)
        {
            return false;
        }
        collectionWatcher.Update();
        var changes = collectionWatcher.GetAndClearChanges();
        if (changes.Count == 0)
        {
            return false;
        }
        if (changes[0].Action == NotifyCollectionChangedAction.Reset)
        {
            // We don't actually need to completely recreate the list of child nodes when a reset happened, only update
            // their context bindings, since all nodes are identical in other respects.
            int index = 0;
            foreach (var item in collectionWatcher.GetCurrentItems())
            {
                IViewNode node;
                if (index >= childNodes.Count)
                {
                    node = innerNodeCreator();
                    childNodes.Add(node);
                }
                else
                {
                    node = childNodes[index];
                }
                node.Context = item is not null ? BindingContext.Create(item) : null;
                index++;
            }
            childNodes.RemoveRange(index, childNodes.Count - index);
            return true;
        }
        foreach (var change in changes)
        {
            // N.B. The conditions below each make some assumptions about the event data being semantically correct.
            // If we got a broken implementation of INotifyCollectionChanged, then these will try not to crash outright,
            // but likely won't produce the expected results.
            switch (change.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddChildNodes(change.NewStartingIndex, change.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    childNodes.RemoveRange(change.OldStartingIndex, change.OldItems?.Count ?? 0);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveChildNodes(change.OldStartingIndex, change.NewStartingIndex, change.OldItems?.Count ?? 0);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceChildNodes(change.OldStartingIndex, change.OldItems, change.NewItems);
                    break;
            }
        }
        return true;
    }

    interface ICollectionWatcher : IDisposable
    {
        IReadOnlyList<NotifyCollectionChangedEventArgs> GetAndClearChanges();

        // Boxing is OK here because they are going to be boxed anyway when we assign them as a Context.
        IEnumerable<object> GetCurrentItems();

        void Update();
    }

    static class CollectionWatcher
    {
        delegate ICollectionWatcher Factory(IValueSource collectionSource);

        private static readonly MethodInfo createInternalMethod = typeof(CollectionWatcher).GetMethod(
            nameof(CreateInternal),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;
        private static readonly Dictionary<Type, Factory> factoryCache = [];

        public static ICollectionWatcher Create(IValueSource collectionSource)
        {
            if (!factoryCache.TryGetValue(collectionSource.ValueType, out var factory))
            {
                var elementType =
                    collectionSource
                        .ValueType.GetInterfaces()
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Select(t => t.GetGenericArguments()[0])
                        .Where(e => e is not null)
                        .FirstOrDefault()
                    ?? throw new BindingException(
                        $"Repeater node cannot bind to non-enumerable type {collectionSource.ValueType.Name}."
                    );
                factory = createInternalMethod
                    .MakeGenericMethod(collectionSource.ValueType, elementType)
                    .CreateDelegate<Factory>();
                factoryCache.Add(collectionSource.ValueType, factory);
            }
            return factory(collectionSource);
        }

        private static ICollectionWatcher CreateInternal<TCollection, TElement>(IValueSource collectionSource)
            where TCollection : IEnumerable<TElement>
        {
            return new CollectionWatcher<TCollection, TElement>((IValueSource<TCollection>)collectionSource);
        }
    }

    class CollectionWatcher<TCollection, TElement> : ICollectionWatcher, IDisposable
        where TCollection : IEnumerable<TElement>
    {
        private static readonly NotifyCollectionChangedEventArgs[] emptyEvents = [];
        private static readonly NotifyCollectionChangedEventArgs resetEvent = new(NotifyCollectionChangedAction.Reset);

        private readonly IValueSource<TCollection> collectionSource;

        private List<NotifyCollectionChangedEventArgs> pendingEvents = [];

        public CollectionWatcher(IValueSource<TCollection> collectionSource)
        {
            this.collectionSource = collectionSource;
            if (collectionSource.Value is not null && collectionSource.Value.Any())
            {
                pendingEvents.Add(resetEvent);
            }
        }

        public void Dispose()
        {
            if (collectionSource.Value is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= Value_CollectionChanged;
            }
            pendingEvents = [];
            GC.SuppressFinalize(this);
        }

        public IReadOnlyList<NotifyCollectionChangedEventArgs> GetAndClearChanges()
        {
            // This method is going to be called on every update. Most of the time, there won't be any updates. In this
            // case, we want to avoid any mutations OR allocations.
            if (pendingEvents.Count == 0)
            {
                return emptyEvents;
            }
            var result = pendingEvents;
            pendingEvents = [];
            return result;
        }

        public IEnumerable<object> GetCurrentItems()
        {
            return collectionSource.Value?.Cast<object>().AsEnumerable() ?? [];
        }

        public void Update()
        {
            var previousValue = collectionSource.Value;
            if (collectionSource.Update())
            {
                if (previousValue is INotifyCollectionChanged previousNotifyCollectionChanged)
                {
                    previousNotifyCollectionChanged.CollectionChanged -= Value_CollectionChanged;
                }
                // Previous sub-collection events no longer matter if the entire collection changed, as the repeater
                // must rebuild its entire tree; we can treat it the same as if the original collection was reset.
                pendingEvents.Clear();
                pendingEvents.Add(resetEvent);
                if (collectionSource.Value is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += Value_CollectionChanged;
                }
            }
        }

        private void Value_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Any time a reset happens, we can discard all previous events since the view tree must be rebuilt.
            // This means instead of having to scan the whole list, the RepeaterNode can simply check the first element.
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                pendingEvents.Clear();
            }
            else if (pendingEvents.Count == 0 || pendingEvents[0].Action != NotifyCollectionChangedAction.Reset)
            {
                pendingEvents.Add(e);
            }
        }
    }
}
