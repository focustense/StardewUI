using System.Text;
using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Quasi-structural node that loads its content from a shared game asset.
/// </summary>
public class IncludedViewNode(
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IAssetCache assetCache,
    Func<Document, IViewNode> nodeCreator,
    IAttribute assetNameAttribute,
    IAttribute? contextAttribute
) : IViewNode
{
    /// <inheritdoc />
    public IReadOnlyList<IViewNode> ChildNodes => childNode is not null ? [childNode] : [];

    /// <inheritdoc />
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

    /// <inheritdoc />
    public IReadOnlyList<IView> Views => childNode?.Views ?? [];

    private IAssetCacheEntry<Document>? assetCacheEntry;
    private string? assetName;
    private IValueSource<string>? assetNameSource;
    private IValueSource? childContextSource;
    private IViewNode? childNode;
    private BindingContext? context;
    private bool wasContextChanged;

    /// <inheritdoc />
    public void Dispose()
    {
        Reset();
        if (childContextSource is IDisposable childContextSourceDisposable)
        {
            childContextSourceDisposable.Dispose();
        }
        childContextSource = null;
        if (assetNameSource is IDisposable assetNameSourceDisposable)
        {
            assetNameSourceDisposable.Dispose();
        }
        assetNameSource = null;
        context = null;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Print(StringBuilder sb, bool includeChildren)
    {
        sb.Append("<include ");
        assetNameAttribute.Print(sb);
        if (contextAttribute is not null)
        {
            sb.Append(' ');
            contextAttribute.Print(sb);
        }
        sb.Append("/>");
    }

    /// <inheritdoc />
    public void Reset()
    {
        childNode?.Dispose();
        childNode = null;
        assetCacheEntry = null;
        assetName = null;
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
        if (wasContextChanged)
        {
            assetNameSource = TryCreateAssetNameSource();
            childContextSource = TryCreateChildContextSource();
            wasContextChanged = false;
        }
        bool wasChanged = false;
        assetNameSource?.Update();
        if (assetNameSource?.Value != assetName || assetCacheEntry?.IsValid != true)
        {
            assetName = assetNameSource?.Value;
            var childNode = TryCreateChildNode();
            if (this.childNode != childNode)
            {
                wasChanged = true;
                this.childNode = childNode;
            }
        }
        childContextSource?.Update();
        if (childNode is not null)
        {
            childNode.Context = contextAttribute is not null
                ? childContextSource?.Value is not null
                    ? BindingContext.Create(childContextSource.Value, context)
                    : null
                : context;
            wasChanged |= childNode.Update(elapsed);
        }
        return wasChanged;
    }

    private IValueSource<string>? TryCreateAssetNameSource()
    {
        using var _ = Trace.Begin(this, nameof(TryCreateAssetNameSource));
        var assetValueType = valueSourceFactory.GetValueType(assetNameAttribute, null, context);
        if (assetValueType is null)
        {
            return null;
        }
        var rawAssetValueSource = valueSourceFactory.GetValueSource(assetNameAttribute, context, assetValueType);
        return ConvertedValueSource.Create<string>(rawAssetValueSource, valueConverterFactory);
    }

    private IValueSource? TryCreateChildContextSource()
    {
        if (contextAttribute is null)
        {
            return null;
        }
        using var _ = Trace.Begin(this, nameof(TryCreateChildContextSource));
        var childContextType = valueSourceFactory.GetValueType(contextAttribute, null, context);
        return childContextType is not null
            ? valueSourceFactory.GetValueSource(contextAttribute, context, childContextType)
            : null;
    }

    private IViewNode? TryCreateChildNode()
    {
        if (string.IsNullOrWhiteSpace(assetName))
        {
            return null;
        }
        using var _ = Trace.Begin(this, nameof(TryCreateChildNode));
        assetCacheEntry = assetCache.Get<Document>(assetName);
        return nodeCreator(assetCacheEntry.Asset);
    }
}
