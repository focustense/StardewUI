using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace StardewUI.Framework.Converters;

/// <summary>
/// Factory that automatically implements duck-typing conversions between enum types that share the same names.
/// </summary>
/// <remarks>
/// Enum values do not need to be identical; matching is performed on the (case-insensitive) name.
/// </remarks>
public class DuckTypeEnumConverterFactory : IValueConverterFactory
{
    /// <inheritdoc />
    public bool TryGetConverter<TSource, TDestination>(
        [MaybeNullWhen(false)] out IValueConverter<TSource, TDestination> converter
    )
    {
        if (!typeof(TSource).IsEnum || !typeof(TDestination).IsEnum)
        {
            converter = null;
            return false;
        }
        // Using Enum.GetNames (etc.) would be more convenient here, but then we'd have to use reflection anyway to pass
        // the Enum constraints in order to call the generic versions of Enum.GetValue<T>, etc.
        // https://github.com/dotnet/csharplang/discussions/6308
        //
        // It ends up being faster to only do this one time.
        var sourceFields = typeof(TSource).GetFields(BindingFlags.Public | BindingFlags.Static);
        var destinationFields = typeof(TDestination).GetFields(BindingFlags.Public | BindingFlags.Static);
        converter = sourceFields
            .Select(f => f.Name)
            .Intersect(destinationFields.Select(f => f.Name), StringComparer.OrdinalIgnoreCase)
            .Any()
            ? new Converter<TSource, TDestination>(sourceFields, destinationFields)
            : null;
        return converter is not null;
    }

    class Converter<TSource, TDestination>(FieldInfo[] sourceFields, FieldInfo[] destinationNames)
        : IValueConverter<TSource, TDestination>
    {
        private readonly Dictionary<object, object> valueMap = sourceFields
            .Join(
                destinationNames,
                f => f.Name,
                f => f.Name,
                (f1, f2) => (f1.GetValue(null)!, f2.GetValue(null)!),
                StringComparer.OrdinalIgnoreCase
            )
            .ToDictionary(x => x.Item1, x => x.Item2);

        public TDestination Convert(TSource value)
        {
            return valueMap.TryGetValue(value!, out var converted) ? (TDestination)converted : (TDestination)(object)0;
        }
    }
}
