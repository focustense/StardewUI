namespace StardewUI.Framework;

/// <summary>
/// Type extensions/helpers used in various reflection code.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// For a given type, attempts to infer the element type of any implemented <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    /// <param name="type">The type which may or may not implement <see cref="IEnumerable{T}"/>.</param>
    /// <returns>The <c>T</c> type parameter of the <see cref="IEnumerable{T}"/> interface implemented by the given
    /// <paramref name="type"/>, or <c>null</c> if no such implementation exists.</returns>
    public static Type? GetEnumerableElementType(this Type type)
    {
        return GetSelfEnumerableElementType(type)
            ?? type.GetInterfaces().Select(GetSelfEnumerableElementType).Where(e => e is not null).FirstOrDefault();
    }

    private static Type? GetSelfEnumerableElementType(Type t)
    {
        return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            ? t.GetGenericArguments()[0]
            : null;
    }
}
