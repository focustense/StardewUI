using Microsoft.Xna.Framework;

namespace StardewUI;

/// <summary>
/// Renders a single-line numeric label using custom digit sprites.
/// </summary>
/// <remarks>
/// <para>
/// Corresponds to <see cref="StardewValley.Utility.drawTinyDigits"/>.
/// </para>
/// <para>
/// For this widget type, <paramref name="baseScale"/> and <see cref="Scale"/> <b>do</b> affect layout, and the size of
/// the rendered text is entirely based on the <paramref name="digitSprites"/> and cumulative scale which is effectively
/// treated like a font size. If the view's <see cref="View.Layout"/> uses any non-content-based dimensions, it will
/// affect the box size as expected but will not change the rendered text; the text is not scaled to the layout bounds.
/// </para>
/// </remarks>
/// <param name="digitSprites">The sprites for each individual digit, with the index corresponding to the digit itself
/// (element 0 for digit '0', element 4 for digit '4', etc.). This must have exactly 10 elements.</param>
/// <param name="baseScale">Scale to apply to the base dimensions of the <paramref name="digitSprites"/> before any
/// extra <see cref="Scale"/>.</param>
public class TinyNumberLabel(IReadOnlyList<Sprite>? digitSprites = null, float baseScale = 2.0f) : View
{
    /// <summary>
    /// The number to display.
    /// </summary>
    public int Number
    {
        get => number.Value;
        set
        {
            if (number.SetIfChanged(value))
            {
                digits = [.. GetDigits(value)];
                OnPropertyChanged(nameof(Number));
            }
        }
    }

    /// <summary>
    /// Custom scale amount, cumulative with the <c>baseScale</c> that the label was constructed with.
    /// </summary>
    /// <remarks>
    /// For example, a <c>baseScale</c> of <c>5.0f</c> (default) and a <see cref="Scale"/> of <c>2.0f</c> would yield a
    /// layout/drawing scale of <c>10.0f</c>, which would render a 5x7 sprite (the vanilla digit sprite size) as 50x70.
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

    private readonly float baseScale = baseScale;
    private readonly IReadOnlyList<Sprite> digitSprites = digitSprites is not null
        ? digitSprites.Count == 10
            ? digitSprites
            : throw new ArgumentException(
                $"Digit sprite list has the wrong number of sprites (expected 10, got {digitSprites.Count}).",
                nameof(digitSprites)
            )
        : UiSprites.Digits;
    private readonly DirtyTracker<int> number = new(0);
    private readonly DirtyTracker<float> scale = new(1.0f);

    private Rectangle[] digitRects = [];
    private int[] digits = [];

    protected override bool IsContentDirty()
    {
        return number.IsDirty || scale.IsDirty;
    }

    protected override void OnDrawContent(ISpriteBatch b)
    {
        for (int i = 0; i < digits.Length; i++)
        {
            var digitSprite = digitSprites[digits[i]];
            var destinationRect = digitRects[i];
            b.Draw(digitSprite.Texture, destinationRect, digitSprite.SourceRect, Color.White);
        }
    }

    protected override void OnMeasure(Vector2 availableSize)
    {
        int totalWidth = 0;
        int maxHeight = 0;
        digitRects = new Rectangle[digits.Length];
        for (int i = 0; i < digits.Length; i++)
        {
            var digitSprite = digitSprites[digits[i]];
            var size = digitSprite.SourceRect?.Size ?? digitSprite.Texture.Bounds.Size;
            int digitWidth = (int)(size.X * baseScale * Scale);
            int digitHeight = (int)(size.Y * baseScale * Scale);
            digitRects[i] = new(totalWidth, 0, digitWidth, digitHeight);
            totalWidth += digitWidth;
            maxHeight = Math.Max(maxHeight, digitHeight);
        }
        ContentSize = Layout.Resolve(availableSize, () => new(totalWidth, maxHeight));
    }

    protected override void ResetDirty()
    {
        number.ResetDirty();
        scale.ResetDirty();
    }

    private static int GetDigit(int number, int position)
    {
        // Switch-based solution isn't elegant, but is vastly more performant than involving any exponential/logarithmic
        // or floating-point arithmetic, and slightly faster than a lookup table since the compiler can optimize the
        // divisions to multiplications.
        var n = position switch
        {
            0 => number,
            1 => number / 10,
            2 => number / 100,
            3 => number / 1000,
            4 => number / 10_000,
            5 => number / 100_000,
            6 => number / 1_000_000,
            7 => number / 10_000_000,
            8 => number / 100_000_000,
            9 => number / 1_000_000_000,
            _ => throw new ArgumentOutOfRangeException(
                nameof(position),
                $"{position} is not a valid digit position for an integer."
            ),
        };
        return n % 10;
    }

    private static IEnumerable<int> GetDigits(int number)
    {
        bool hasYielded = false;
        for (int i = 9; i >= 0; i--)
        {
            var digit = GetDigit(number, i);
            if (digit == 0 && !hasYielded)
            {
                continue;
            }
            yield return digit;
            hasYielded = true;
        }
    }
}
