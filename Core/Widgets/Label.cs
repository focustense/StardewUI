﻿using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// A view that renders a read-only text string.
/// </summary>
public partial class Label : View
{
    /// <summary>
    /// Creates a typical, simple run of 1-line text using content sizing.
    /// </summary>
    /// <param name="initialText">Initial text to display; can be updated later.</param>
    /// <param name="font">Font to use, if different from the default label font.</param>
    /// <param name="color">Color to use, if different from the default font color.</param>
    /// <param name="margin">Horizontal margin to add.</param>
    /// <returns></returns>
    public static Label Simple(string initialText, SpriteFont? font = null, Color? color = null, int margin = 0)
    {
        var label = new Label()
        {
            Layout = LayoutParameters.FitContent(),
            Margin = new(margin, 0),
            MaxLines = 1,
            Text = initialText,
        };
        if (font is not null)
        {
            label.Font = font;
        }
        if (color is not null)
        {
            label.Color = color.Value;
        }
        return label;
    }

    /// <summary>
    /// Whether to draw the text in a bold style.
    /// </summary>
    /// <remarks>
    /// Current implementation is based on overdraw, as <see cref="SpriteFont"/> does not support font variants. Changing
    /// this setting will not affect size/layout.
    /// </remarks>
    public bool Bold
    {
        get => bold;
        set
        {
            if (value != bold)
            {
                bold = value;
                OnPropertyChanged(nameof(Bold));
            }
        }
    }

    /// <summary>
    /// The text color.
    /// </summary>
    public Color Color
    {
        get => color;
        set
        {
            if (value != color)
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
    }

    /// <summary>
    /// The font that will be used to render the text.
    /// </summary>
    public SpriteFont Font
    {
        get => font.Value;
        set
        {
            if (font.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Font));
            }
        }
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
    /// </code>
    /// While left-aligned text that is centered in the container looks like:
    /// <code>
    /// +--------------------------------------------+
    /// |         The quick brown fox jumps          |
    /// |         over the lazy dog                  |
    /// +--------------------------------------------+
    /// </code>
    /// </example>
    /// Alignment behavior is also sensitive to the width settings in <see cref="View.Layout"/>.
    /// <see cref="Alignment.Middle"/> and <see cref="Alignment.End"/> may have no effect if the width type is set to
    /// <see cref="LengthType.Content"/>; for non-default alignments to work, one of the other length types is required.
    /// </remarks>
    public Alignment HorizontalAlignment
    {
        get => horizontalAlignment;
        set
        {
            if (value != horizontalAlignment)
            {
                horizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }
    }

    /// <summary>
    /// Maximum number of lines of text to display when wrapping. Default is <c>0</c> which applies no limit.
    /// </summary>
    public int MaxLines
    {
        get => maxLines.Value;
        set
        {
            if (maxLines.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(MaxLines));
            }
        }
    }

    /// <summary>
    /// Font scaling to apply. Default is <c>1.0</c> (normal size).
    /// </summary>
    /// <remarks>
    /// Applies only to the text itself and not layout properties such as <see cref="View.Margin"/>.
    /// </remarks>
    public float Scale
    {
        get => scale.Value;
        set
        {
            if (scale.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Scale));
            }
        }
    }

    /// <summary>
    /// Alpha value for the text shadow, per layer in <see cref="ShadowLayers"/>.
    /// </summary>
    /// <remarks>
    /// If set to zero, no text shadow will be drawn.
    /// </remarks>
    public float ShadowAlpha
    {
        get => shadowAlpha;
        set
        {
            if (value != shadowAlpha)
            {
                shadowAlpha = value;
                OnPropertyChanged(nameof(ShadowAlpha));
            }
        }
    }

    /// <summary>
    /// Base color for the text shadow, before applying <see cref="ShadowAlpha"/>.
    /// </summary>
    public Color ShadowColor
    {
        get => shadowColor;
        set
        {
            if (value != shadowColor)
            {
                shadowColor = value;
                OnPropertyChanged(nameof(shadowColor));
            }
        }
    }

    /// <summary>
    /// Specifies which layers of the shadow should be drawn.
    /// </summary>
    /// <remarks>
    /// Layers are additive, so the same <see cref="ShadowAlpha"/> will have a different visual intensity depending on
    /// which layers are allowed. If set to <see cref="ShadowLayers.None"/>, then no shadow will be drawn.
    /// </remarks>
    public ShadowLayers ShadowLayers
    {
        get => shadowLayers;
        set
        {
            if (value != shadowLayers)
            {
                shadowLayers = value;
                OnPropertyChanged(nameof(ShadowLayers));
            }
        }
    }

    /// <summary>
    /// Offset to draw the text shadow, which is a second copy of the <see cref="Text"/> drawn entirely black.
    /// Text shadows will not be visible unless <see cref="ShadowAlpha"/> is non-zero.
    /// </summary>
    public Vector2 ShadowOffset
    {
        get => shadowOffset;
        set
        {
            if (value != shadowOffset)
            {
                shadowOffset = value;
                OnPropertyChanged(nameof(ShadowOffset));
            }
        }
    }

    /// <summary>
    /// The text string to display.
    /// </summary>
    public string Text
    {
        get => text.Value;
        set
        {
            if (text.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    /// <inheritdoc />
    protected override bool HandlesOpacity => true;

    private record HyphenationOptions(string Hyphen, float Width, Regex? NoBreakRegex);

    private static readonly Regex CjkCharacterRegex = new(
        @"\p{IsHangulJamo}|"
            + @"\p{IsCJKRadicalsSupplement}|"
            + @"\p{IsCJKSymbolsandPunctuation}|"
            + @"\p{IsEnclosedCJKLettersandMonths}|"
            + @"\p{IsCJKCompatibility}|"
            + @"\p{IsCJKUnifiedIdeographsExtensionA}|"
            + @"\p{IsCJKUnifiedIdeographs}|"
            + @"\p{IsHangulSyllables}|"
            + @"\p{IsCJKCompatibilityForms}",
        RegexOptions.Compiled
    );

    private static readonly Regex PunctuationRegex = new(@"\p{P}", RegexOptions.Compiled);

    private static readonly ShadowLayers[] ShadowLayerOrder =
    [
        ShadowLayers.Diagonal,
        ShadowLayers.Vertical,
        ShadowLayers.Horizontal,
    ];

    private readonly DirtyTracker<SpriteFont> font = new(Game1.smallFont);
    private readonly DirtyTracker<int> maxLines = new(0);
    private readonly DirtyTracker<float> scale = new(1.0f);
    private readonly DirtyTracker<string> text = new("");

    private bool bold; // Not dirty-tracked because it doesn't affect layout.
    private Color color = Game1.textColor; // Not dirty-tracked because it doesn't affect layout.
    private Alignment horizontalAlignment; // Not dirty-tracked as it doesn't change the max line width.
    private List<string> lines = [];
    private float shadowAlpha;
    private Color shadowColor = Game1.textShadowDarkerColor;
    private ShadowLayers shadowLayers = ShadowLayers.All;
    private Vector2 shadowOffset = new(-2, 2);

    /// <inheritdoc />
    protected override bool HasOwnContent()
    {
        return !string.IsNullOrWhiteSpace(Text);
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        if (ShadowAlpha > 0 && ShadowLayers > 0)
        {
            var shadowAlphaColor = ShadowColor * ShadowAlpha * Opacity;
            foreach (var layer in ShadowLayerOrder)
            {
                if ((ShadowLayers & layer) == 0)
                {
                    continue;
                }
                using var _ = b.SaveTransform();
                b.Translate(
                    layer switch
                    {
                        ShadowLayers.Diagonal => ShadowOffset,
                        ShadowLayers.Horizontal => new(ShadowOffset.X, 0),
                        ShadowLayers.Vertical => new(0, ShadowOffset.Y),
                        _ => throw new InvalidOperationException($"Invalid shadow layer {layer}"),
                    }
                );
                DrawText(shadowAlphaColor);
            }
        }
        DrawText(Color * Opacity);

        void DrawText(Color color)
        {
            var y = 0;
            foreach (var line in lines)
            {
                var x = GetAlignedLeft(line);
                b.DrawString(Font, line, new(x, y), color, scale: Scale);
                if (Bold)
                {
                    b.DrawString(Font, line, new(x + 1, y), color, scale: Scale);
                    b.DrawString(Font, line, new(x, y + 1), color, scale: Scale);
                    b.DrawString(Font, line, new(x + 1, y + 1), color, scale: Scale);
                }
                y += Font.LineSpacing;
            }
        }
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return font.IsDirty || maxLines.IsDirty || scale.IsDirty || text.IsDirty;
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        // For text, we need to always perform the line-breaking algorithm on layout (so that it is
        // available on draw) even if the layout size is not content-dependent.
        var maxTextSize = Layout.GetLimits(availableSize);
        BreakLines(maxTextSize.X, out var maxLineWidth);
        ContentSize = Layout.Resolve(availableSize, () => new(maxLineWidth, lines.Count * Font.LineSpacing * Scale));
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        font.ResetDirty();
        maxLines.ResetDirty();
        scale.ResetDirty();
        text.ResetDirty();
    }

    private void BreakLines(float availableWidth, out float maxLineWidth)
    {
        // To incorporate font scaling more cheaply, without having to perform float multiplications for every word,
        // we can instead invert the scaling on available width, and reapply it at the end.
        availableWidth /= Scale;
        var rawLines = Text.Replace("\r\n", "\n").Split('\n').Select(line => line.Split(' ')).ToList();
        // Greedy breaking algorithm. Knuth *probably* isn't necessary in a use case like this?
        maxLineWidth = 0.0f;
        lines = [];
        var spaceWidth = Font.MeasureString(" ").X;
        HyphenationOptions? hyphenationOptions = null;
        foreach (var line in rawLines)
        {
            var sb = new StringBuilder();
            var remainingWidth = availableWidth;
            // Track isFirstWord explicitly instead of checking sb.Length == 0 because the first "word" can be empty when
            // there is a leading space - and leading spaces should actually render, it's not our job to trim here.
            bool isFirstWord = true;
            var wordEnumerator = line.GetEnumerator();
            string? continuedWord = null;
            while (!string.IsNullOrEmpty(continuedWord) || wordEnumerator.MoveNext())
            {
                string word = !string.IsNullOrEmpty(continuedWord) ? continuedWord : (string)wordEnumerator.Current;
                continuedWord = null;
                var wordWidth = Font.MeasureString(word).X;
                if (isFirstWord || remainingWidth >= wordWidth + spaceWidth)
                {
                    if (!isFirstWord)
                    {
                        sb.Append(' ');
                        remainingWidth -= spaceWidth;
                    }
                }
                else
                {
                    if (!AppendCurrentLine(ref maxLineWidth, true))
                    {
                        return;
                    }
                }
                if (isFirstWord && wordWidth > remainingWidth)
                {
                    // Most strings should hopefully not require word-breaking, so we can defer the (expensive) regex
                    // until it's definitely going to happen.
                    hyphenationOptions ??= ContainsCjkCharacters(Text)
                        ? new("", 0, PunctuationRegex)
                        : new HyphenationOptions("-", Font.MeasureString("-").X, PunctuationRegex);
                    string brokenWord = BreakWord(ref word, ref wordWidth, remainingWidth, hyphenationOptions);
                    continuedWord = word;
                    word = brokenWord;
                    // Only append the hyphen if it's not going to be ellipsized later. We should add a hyphen OR
                    // ellipsis, not both. Also, only hyphenate if breaking succeeded.
                    if (
                        continuedWord.Length > 0
                        && hyphenationOptions.Hyphen.Length > 0
                        && (MaxLines == 0 || lines.Count < (MaxLines - 1))
                    )
                    {
                        word += hyphenationOptions.Hyphen;
                        wordWidth += hyphenationOptions.Width;
                    }
                }
                sb.Append(word);
                remainingWidth -= wordWidth;
                isFirstWord = false;
            }
            AppendCurrentLine(ref maxLineWidth, false);

            bool AppendCurrentLine(ref float maxLineWidth, bool ellipsize)
            {
                var fittedLine = sb.ToString();
                // It might seem mathematically that we can use "availableWidth - remainingWidth" as the line width
                // here, but in fact this is inaccurate because of kerning. Instead we need to re-measure the entire
                // line in order to get an accurate width.
                // Technically, this means a line with many spaces might get broken earlier than it needs to be,
                // possibly with the resulting label using more lines than it needs to use. In practice, this tends
                // to be a lot less noticeable of an issue than having a wrong final content size on single-line
                // text (where the more spaces are added, the bigger a "phantom margin" appears between the text and
                // whatever follows it in the layout).
                var lineWidth = Font.MeasureString(fittedLine).X * Scale;
                maxLineWidth = MathF.Max(maxLineWidth, lineWidth);
                lines.Add(fittedLine);
                if (MaxLines > 0 && lines.Count == MaxLines)
                {
                    if (ellipsize)
                    {
                        lines[^1] = Ellipsize(lines[^1], availableWidth);
                    }
                    return false;
                }
                sb.Clear();
                remainingWidth = availableWidth;
                isFirstWord = true;
                return true;
            }
        }
        maxLineWidth *= Scale;
    }

    private string BreakWord(ref string word, ref float wordWidth, float availableWidth, HyphenationOptions options)
    {
        var runes = word.EnumerateRunes().ToArray().AsSpan();
        string result = runes[0].ToString();
        int minLength = 1;
        int maxLength = runes.Length;
        int nextLength;
        wordWidth = 0;
        do
        {
            nextLength = Math.Max(1, (minLength + maxLength) / 2);
            if (nextLength < maxLength && options.NoBreakRegex is Regex noBreakRegex)
            {
                while (nextLength >= minLength && noBreakRegex.IsMatch(runes[nextLength].ToString()))
                {
                    nextLength--;
                }
            }
            string brokenWord = Encoding.UTF32.GetString(MemoryMarshal.Cast<Rune, byte>(runes[..nextLength]));
            float brokenWordWidth = Font.MeasureString(brokenWord).X;
            if (brokenWordWidth < availableWidth - options.Width)
            {
                minLength = Math.Max(nextLength, minLength + 1);
                result = brokenWord;
                wordWidth = brokenWordWidth;
            }
            else
            {
                maxLength = Math.Min(nextLength, maxLength - 1);
            }
        } while (maxLength > minLength);
        if (wordWidth == 0)
        {
            wordWidth = Font.MeasureString(result).X;
        }
        word = word[result.Length..];
        return result;
    }

    private static string Ellipsize(string text, float maxWidth)
    {
        // There's a chance that adding the ellipsis could make the line too long; we're ignoring that
        // for the time being. If it causes serious issues later on, the fix would be to trim 1-2
        // characters at a time and re-measure until the line is short enough.
        // In practice, this is unlikely to happen because of the previous issue - any line that
        // actually spaces will break slightly sooner than the true formatted width dictates.
        return text + " ...";
    }

    private float GetAlignedLeft(string text)
    {
        switch (HorizontalAlignment)
        {
            case Alignment.Start:
                return 0;
            case Alignment.Middle:
                var textWidth = Font.MeasureString(text).X * Scale;
                return ContentSize.X / 2 - textWidth / 2;
            case Alignment.End:
                textWidth = Font.MeasureString(text).X * Scale;
                return ContentSize.X - textWidth;
            default:
                throw new NotImplementedException($"Invalid alignment type: {HorizontalAlignment}");
        }
    }

    private static bool ContainsCjkCharacters(string text)
    {
        return CjkCharacterRegex.IsMatch(text);
    }
}
