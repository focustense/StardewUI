using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Text;

namespace SupplyChain.UI;

/// <summary>
/// A view that renders a read-only text string.
/// </summary>
public class Label : View
{
    /// <summary>
    /// The text color.
    /// </summary>
    public Color Color { get; set; } // Not dirty-tracked because it doesn't affect layout.

    /// <summary>
    /// The font that will be used to render the text.
    /// </summary>
    public SpriteFont Font
    {
        get => font.Value;
        set => font.Value = value;
    }

    /// <summary>
    /// How to align the text horizontally.
    /// </summary>
    /// <remarks>
    /// This acts differently from setting an <see cref="Alignment"/> on the container view as it applies to each
    /// individual line of text rather than the entire block of text.
    /// <example>
    /// For example, center-aligned text looks like:
    /// <code>
    /// +--------------------------------------------+
    /// |         The quick brown fox jumps          |
    /// |             over the lazy dog              |
    /// +--------------------------------------------+
    /// test2
    /// </code>
    /// While left-aligned text that is centered in the container looks like:
    /// <code>
    /// +--------------------------------------------+
    /// |         The quick brown fox jumps          |
    /// |         over the lazy dog                  |
    /// +--------------------------------------------+
    /// </code>
    /// </example>
    /// Alignment behavior is also sensitive to the width settings in <see cref="Layout"/>.
    /// <see cref="Alignment.Middle"/> and <see cref="Alignment.End"/> may have no effect if the width type is set to
    /// <see cref="LengthType.Content"/>; for non-default alignments to work, one of the other length types is required.
    /// </remarks>
    public Alignment HorizontalAlignment { get; set; } // Not dirty-tracked as it doesn't change the max line width.

    /// <summary>
    /// Maximum number of lines of text to display when wrapping. Default is <c>0</c> which applies no limit.
    /// </summary>
    public int MaxLines
    {
        get => maxLines.Value;
        set => maxLines.Value = value;
    }

    /// <summary>
    /// The text string to display.
    /// </summary>
    public string Text
    {
        get => text.Value;
        set => text.Value = value;
    }

    private readonly DirtyTracker<SpriteFont> font = new(Game1.smallFont);
    private readonly DirtyTracker<int> maxLines = new(0);
    private readonly DirtyTracker<string> text = new("");

    private List<string> lines = [];

    protected override void OnDrawContent(ISpriteBatch b)
    {
        var y = 0;
        foreach (var line in lines)
        {
            var x = GetAlignedLeft(line);
            b.DrawString(Font, line, new(x, y), Color);
            y += Font.LineSpacing;
        }
    }

    protected override bool IsContentDirty()
    {
        return font.IsDirty || maxLines.IsDirty || text.IsDirty;
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        // For text, we need to always perform the line-breaking algorithm on layout (so that it is
        // available on draw) even if the layout size is not content-dependent.
        var maxTextSize = Layout.GetLimits(availableSize);
        BreakLines(maxTextSize.X, out var maxLineWidth);
        ContentSize = Layout.Resolve(availableSize, () => new(maxLineWidth, lines.Count * Font.LineSpacing));
    }

    protected override void ResetDirty()
    {
        font.ResetDirty();
        maxLines.ResetDirty();
        text.ResetDirty();
    }

    private void BreakLines(float availableWidth, out float maxLineWidth)
    {
        var rawLines = Text
            .Replace("\r\n", "\n")
            .Split('\r')
            .Select(line => line.Split(' '))
            .ToList();
        // Greedy breaking algorithm. Knuth *probably* isn't necessary in a use case like this?
        var spaceWidth = Font.MeasureString(" ").X;
        maxLineWidth = 0.0f;
        lines = [];
        foreach (var line in rawLines)
        {
            var sb = new StringBuilder();
            var remainingWidth = availableWidth;
            foreach (var word in line)
            {
                var wordWidth = Font.MeasureString(word).X;
                if (sb.Length == 0 || remainingWidth >= wordWidth + spaceWidth)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(' ');
                        remainingWidth -= spaceWidth;
                    }
                }
                else
                {
                    maxLineWidth = MathF.Max(maxLineWidth, availableWidth - remainingWidth);
                    lines.Add(sb.ToString());
                    if (MaxLines > 0 && lines.Count == MaxLines)
                    {
                        return;
                    }
                    sb.Clear();
                    remainingWidth = availableWidth;
                }
                sb.Append(word);
                remainingWidth -= wordWidth;
            }
            maxLineWidth = MathF.Max(maxLineWidth, availableWidth - remainingWidth);
            lines.Add(sb.ToString());
        }
    }

    private float GetAlignedLeft(string text)
    {
        switch (HorizontalAlignment)
        {
            case Alignment.Start:
                return 0;
            case Alignment.Middle:
                var textWidth = Font.MeasureString(text).X;
                return ContentSize.X / 2 - textWidth / 2;
            case Alignment.End:
                textWidth = Font.MeasureString(text).X;
                return ContentSize.X - textWidth;
            default:
                throw new NotImplementedException($"Invalid alignment type: {HorizontalAlignment}");
        }
    }
}
