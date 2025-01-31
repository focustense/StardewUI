using System.Collections;
using System.Collections.Specialized;

namespace StardewUITest.Examples.Tempering;

internal class TimedListViewModel<T>(TimeSpan duration) : IEnumerable<T>, INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly LinkedList<Entry> entries = [];
    private TimeSpan totalElapsed;

    public void Add(T item)
    {
        entries.AddLast(new Entry(totalElapsed + duration, item));
        CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, item));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return entries.Select(entry => entry.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Update(TimeSpan elapsed)
    {
        totalElapsed += elapsed;
        var removed = new List<T>();
        while (entries.First?.Value.RemovalTime <= totalElapsed)
        {
            removed.Add(entries.First.Value.Value);
            entries.RemoveFirst();
        }
        if (removed.Count > 0)
        {
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, removed, 0));
        }
    }

    private record Entry(TimeSpan RemovalTime, T Value);
}
