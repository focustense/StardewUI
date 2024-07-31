namespace StardewUI;

internal static class EnumerableExtensions
{
    public static (T, IEnumerable<T>) SplitFirst<T>(this IEnumerable<T> source)
    {
        var e = source.GetEnumerator();
        e.MoveNext();
        return (e.Current, e.ToEnumerable());
    }

    public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
    {
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
