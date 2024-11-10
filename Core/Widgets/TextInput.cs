using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewUI.Animation;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Input;
using StardewUI.Layout;
using StardewValley;
using StardewValley.Menus;

namespace StardewUI.Widgets;

/// <summary>
/// A text input field that allows typing from a physical or virtual keyboard.
/// </summary>
public class TextInput : View
{
    /// <summary>
    /// Event raised when the <see cref="Text"/> changes.
    /// </summary>
    public event EventHandler<EventArgs>? TextChanged;

    /// <inheritdoc cref="Frame.Background"/>
    public Sprite? Background
    {
        get => frame.Background;
        set
        {
            if (value != frame.Background)
            {
                frame.Background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }

    /// <summary>
    /// Gets or sets the thickness of the border edges in the <see cref="Background"/> sprite.
    /// </summary>
    /// <remarks>
    /// This is similar to <see cref="Frame.Border"/> but assumes that the border is part of the background, rather than
    /// a separate sprite. Setting this affects padding of content inside the background.
    /// </remarks>
    public Edges BorderThickness
    {
        get => frame.Padding;
        set
        {
            if (value != frame.Padding)
            {
                frame.Padding = value;
                OnPropertyChanged(nameof(BorderThickness));
            }
        }
    }

    /// <summary>
    /// Sprite to draw for the cursor showing the current text position.
    /// </summary>
    public Sprite? Caret
    {
        get => caret.Sprite;
        set
        {
            if (value != caret.Sprite)
            {
                caret.Sprite = value;
                OnPropertyChanged(nameof(Caret));
            }
        }
    }

    /// <summary>
    /// The zero-based position of the caret within the text.
    /// </summary>
    /// <remarks>
    /// This value is the string position; e.g. if the <see cref="Text"/> has a length of 5, and the current caret
    /// position is 2, then the caret is between the 2nd and 3rd characters. The value cannot be greater than the length
    /// of the current text.
    /// </remarks>
    public int CaretPosition
    {
        get => labelBeforeCursor.Text.Length;
        set
        {
            if (SetCaretPosition(value))
            {
                OnPropertyChanged(nameof(CaretPosition));
            }
        }
    }

    /// <summary>
    /// The width to draw the <see cref="Caret"/>, if different from the sprite's source width.
    /// </summary>
    public float? CaretWidth
    {
        get => caret.Layout.Width.Type == LengthType.Px ? caret.Layout.Width.Value : null;
        set
        {
            if (value != CaretWidth)
            {
                caret.Layout = new()
                {
                    Width = value.HasValue ? Length.Px(value.Value) : Length.Content(),
                    Height = Length.Stretch(),
                };
                OnPropertyChanged(nameof(CaretWidth));
            }
        }
    }

    /// <summary>
    /// The font with which to render text. Defaults to <see cref="Game1.smallFont"/>.
    /// </summary>
    public SpriteFont Font
    {
        get => labelAfterCursor.Font;
        set
        {
            if (value != labelAfterCursor.Font || value != labelBeforeCursor.Font)
            {
                labelAfterCursor.Font = value;
                labelBeforeCursor.Font = value;
                OnPropertyChanged(nameof(Font));
            }
        }
    }

    /// <summary>
    /// The maximum number of characters allowed in this field.
    /// </summary>
    /// <remarks>
    /// The default value is <c>0</c> which does not impose any limit.
    /// </remarks>
    public int MaxLength
    {
        get => maxLength;
        set
        {
            if (value != maxLength)
            {
                maxLength = value;
                if (value > 0 && Text.Length > value)
                {
                    Text = Text[..value];
                }
                OnPropertyChanged(nameof(MaxLength));
            }
        }
    }

    /// <inheritdoc cref="Frame.ShadowAlpha"/>
    public float ShadowAlpha
    {
        get => frame.ShadowAlpha;
        set
        {
            if (value != frame.ShadowAlpha)
            {
                frame.ShadowAlpha = value;
                OnPropertyChanged(nameof(ShadowAlpha));
            }
        }
    }

    /// <inheritdoc cref="Frame.ShadowOffset"/>
    public Vector2 ShadowOffset
    {
        get => frame.ShadowOffset;
        set
        {
            if (value != frame.ShadowOffset)
            {
                frame.ShadowOffset = value;
                OnPropertyChanged(nameof(ShadowOffset));
            }
        }
    }

    /// <summary>
    /// Color of displayed text, as well as the <see cref="Caret"/> tint color.
    /// </summary>
    public Color TextColor
    {
        get => labelBeforeCursor.Color;
        set
        {
            if (value != labelBeforeCursor.Color)
            {
                labelBeforeCursor.Color = value;
                labelAfterCursor.Color = value;
                caret.Tint = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }
    }

    /// <summary>
    /// The text currently entered.
    /// </summary>
    /// <remarks>
    /// Setting this to a new value will reset the caret position to the end of the text.
    /// </remarks>
    public string Text
    {
        get => labelBeforeCursor.Text + labelAfterCursor.Text;
        set => SetText(value);
    }

    // A very small positive offset we add to the search position when trying to move the caret to the mouse cursor.
    // In general, the caret should always move BEFORE the character that was clicked on; however, this has a tendency
    // to "overshoot" if the user tries to click between two characters (as many are accustomed to doing). To
    // compensate, we shift the position slightly to the right.
    //
    // Note: We have to be careful not to overdo this in case of a very thin character, like "i" or "l". If the offset
    // is bigger or almost as big as the actual character width, we'll just miss it entirely.
    private const float CARET_SEARCH_OFFSET = 4.0f;

    private readonly Image caret;
    private readonly Animator<Image, Visibility> caretBlinkAnimator;
    private readonly Frame frame;
    private readonly Label labelAfterCursor;
    private readonly Label labelBeforeCursor;
    private readonly TextBoxInterceptor textBoxInterceptor;
    private readonly TextInputSubscriber textInputSubscriber;

    private int maxLength;

    /// <summary>
    /// Initializes a new <see cref="TextInput"/>.
    /// </summary>
    public TextInput()
    {
        Focusable = true;

        caret = new Image()
        {
            Name = "TextInputCursor",
            Layout = new() { Width = Length.Px(2), Height = Length.Stretch() },
            Margin = new(-2, 0),
            Fit = ImageFit.Stretch,
            Sprite = new(Game1.staminaRect),
            Tint = Game1.textColor,
            Visibility = Visibility.Hidden,
        };
        caretBlinkAnimator = Animator.On(
            caret,
            i => i.Visibility,
            (_, _, progress) => progress < 0.5f ? Visibility.Visible : Visibility.Hidden,
            (i, v) => i.Visibility = v
        );
        caretBlinkAnimator.Loop = true;
        labelBeforeCursor = new()
        {
            Name = "TextInputBeforeCursor",
            Layout = LayoutParameters.FitContent(),
            MaxLines = 1,
        };
        labelAfterCursor = new()
        {
            Name = "TextInputAfterCursor",
            Layout = LayoutParameters.FitContent(),
            MaxLines = 1,
        };
        var textLane = new Lane()
        {
            Name = "TextInputContentLane",
            Layout = LayoutParameters.Fill(),
            VerticalContentAlignment = Alignment.Middle,
            Children = [labelBeforeCursor, caret, labelAfterCursor],
        };
        var textBoxSprite = UiSprites.TextBox;
        frame = new()
        {
            Layout = LayoutParameters.Fill(),
            Padding = textBoxSprite.FixedEdges ?? new(4),
            Background = textBoxSprite,
            Content = textLane,
        };
        textBoxInterceptor = new(this);
        textInputSubscriber = new(this, Game1.keyboardFocusInstance.Window);

        Font = Game1.smallFont;
        TextColor = Game1.textColor;
    }

    /// <inheritdoc />
    protected override IEnumerable<ViewChild> GetLocalChildren()
    {
        return [new(frame, Vector2.Zero)];
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return frame.IsDirty();
    }

    /// <inheritdoc />
    public override void OnClick(ClickEventArgs e)
    {
        if (e.IsPrimaryButton())
        {
            Capture(e.Position);
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        frame.Draw(b);
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        frame.Measure(limits);
        ContentSize = Layout.Resolve(availableSize, () => frame.OuterSize);
    }

    private void Capture(Vector2 cursorPosition)
    {
        Release(); // In case of switch between mouse and controller
        if (Game1.options.gamepadControls && !Game1.lastCursorMotionWasMouse)
        {
            // Vanilla text entry doesn't support moving the caret, so make sure we're at the end.
            CaretPosition = Text.Length;
            textBoxInterceptor.Width = (int)OuterSize.X;
            textBoxInterceptor.Height = (int)OuterSize.Y;
            textBoxInterceptor.Selected = true;
            Game1.showTextEntry(textBoxInterceptor);
        }
        else
        {
            var searchOrigin = new Vector2(BorderThickness.Left - CARET_SEARCH_OFFSET, BorderThickness.Top);
            MoveCaretToCursor(cursorPosition - searchOrigin);
            caretBlinkAnimator.Start(Visibility.Visible, Visibility.Hidden, TimeSpan.FromSeconds(1));
            Game1.keyboardDispatcher.Subscriber = textInputSubscriber;
        }
    }

    private void HandleSpecialKey(Keys key)
    {
        switch (key)
        {
            case Keys.Left:
                CaretPosition--;
                break;
            case Keys.Right:
                CaretPosition++;
                break;
            case Keys.Home:
                CaretPosition = 0;
                break;
            case Keys.End:
                CaretPosition = Text.Length;
                break;
            case Keys.Delete:
                if (labelAfterCursor.Text.Length > 0)
                {
                    labelAfterCursor.Text = labelAfterCursor.Text[1..];
                    OnTextChanged();
                }
                break;
        }
    }

    private void Insert(char c)
    {
        switch (c)
        {
            case '\b':
                if (labelBeforeCursor.Text.Length > 0)
                {
                    labelBeforeCursor.Text = labelBeforeCursor.Text[..^1];
                    OnTextChanged();
                }
                break;
            case '\t':
            case '\r':
                Release();
                break;
            default:
                if (!char.IsControl(c))
                {
                    if (MaxLength == 0 || Text.Length < MaxLength)
                    {
                        labelBeforeCursor.Text += c;
                        OnTextChanged();
                    }
                }
                break;
        }
    }

    private void Insert(string text)
    {
        if (MaxLength > 0)
        {
            var remainingLength = Math.Max(MaxLength - Text.Length, 0);
            if (text.Length > remainingLength)
            {
                text = text[..remainingLength];
            }
        }
        if (text.Length > 0)
        {
            labelBeforeCursor.Text += text;
            OnTextChanged();
        }
    }

    private void MoveCaretToCursor(Vector2 position)
    {
        if (position.X < 0 || position.X > ContentSize.X || Text.Length == 0)
        {
            return;
        }
        // Taking into account proportional widths, bearings, kernings, etc., we know very little about the relationship
        // of pixel positions to character positions and don't want to reimplement the entire font system.
        // A reasonably (?) fast solution should be to actually measure partial strings, using a binary search on the
        // length of the before/after string.
        var (previousCharacterCount, labelText, labelOffset) =
            position.X < labelBeforeCursor.OuterSize.X
                ? (0, labelBeforeCursor.Text, position.X)
                : (labelBeforeCursor.Text.Length, labelAfterCursor.Text, position.X - labelBeforeCursor.OuterSize.X);
        var searchStart = 0;
        var searchEnd = labelText.Length;
        while (searchStart < searchEnd)
        {
            int searchMid = (int)(MathF.Ceiling((searchStart + searchEnd) / 2.0f));
            var searchText = labelText[0..searchMid];
            var textWidth = Font.MeasureString(searchText).X;
            if (labelOffset < textWidth)
            {
                searchEnd = Math.Min(searchEnd - 1, searchMid);
            }
            else
            {
                searchStart = Math.Max(searchStart + 1, searchMid);
            }
        }
        var finalIndex = searchStart;
        CaretPosition = previousCharacterCount + finalIndex;
    }

    private void OnTextChanged()
    {
        TextChanged?.Invoke(this, EventArgs.Empty);
        OnPropertyChanged(nameof(Text));
    }

    private void Release()
    {
        textBoxInterceptor.Selected = false;
        textInputSubscriber.Selected = false;
        Game1.closeTextEntry();
        caretBlinkAnimator.Stop();
        caret.Visibility = Visibility.Hidden;
    }

    private bool SetCaretPosition(int position)
    {
        var fullText = Text;
        position = Math.Clamp(position, 0, fullText.Length);
        if (position == CaretPosition)
        {
            return false;
        }
        labelBeforeCursor.Text = position > 0 ? fullText[0..position] : "";
        labelAfterCursor.Text = position < fullText.Length ? fullText[position..] : "";
        return true;
    }

    private void SetText(string text)
    {
        if (text == Text)
        {
            return;
        }
        if (maxLength > 0 && text.Length > maxLength)
        {
            text = text[..maxLength];
        }
        labelBeforeCursor.Text = text;
        labelAfterCursor.Text = "";
        OnTextChanged();
    }

    private class TextBoxInterceptor(TextInput owner)
        : TextBox(Game1.staminaRect, Game1.staminaRect, Game1.smallFont, Color.Black),
            ICaptureTarget
    {
        private readonly TextInput owner = owner;

        public IView CapturingView => owner;

        public override void Draw(SpriteBatch spriteBatch, bool drawShadow = true)
        {
            var b = new PropagatedSpriteBatch(spriteBatch, Transform.FromTranslation(new(X, Y)));
            owner.Draw(b);
        }

        public override void RecieveCommandInput(char command)
        {
            if (Selected)
            {
                owner.Insert(command);
            }
        }

        public override void RecieveTextInput(char inputChar)
        {
            if (Selected)
            {
                owner.Insert(inputChar);
            }
        }

        public override void RecieveTextInput(string text)
        {
            if (Selected)
            {
                owner.Insert(text);
            }
        }

        public void ReleaseCapture()
        {
            owner.Release();
        }
    }

    // Used for when we *don't* have controller input, thus don't use the virtual keyboard, don't want to accidentally
    // incur any side effects of the TextBoxInterceptor.
    private class TextInputSubscriber(TextInput owner, GameWindow window) : ICaptureTarget, IKeyboardSubscriber
    {
        private readonly TextInput owner = owner;
        private readonly GameWindow window = window;

        public bool Selected
        {
            get => selected;
            set
            {
                if (value == selected)
                {
                    return;
                }
                selected = value;
                if (selected)
                {
                    Game1.keyboardDispatcher.Subscriber = this;
                    if (PlatformUsesWindowEvents())
                    {
                        window.KeyDown += Window_KeyDown;
                    }
                    else
                    {
                        KeyboardInput.KeyDown += KeyboardInput_KeyDown;
                    }
                }
                else
                {
                    if (PlatformUsesWindowEvents())
                    {
                        window.KeyDown -= Window_KeyDown;
                    }
                    else
                    {
                        KeyboardInput.KeyDown -= KeyboardInput_KeyDown;
                    }
                    if (Game1.keyboardDispatcher.Subscriber == this)
                    {
                        Game1.keyboardDispatcher.Subscriber = null;
                    }
                }
            }
        }

        public IView CapturingView => owner;

        private bool selected;

        public void RecieveCommandInput(char command)
        {
            if (Selected)
            {
                owner.Insert(command);
            }
        }

        public void RecieveSpecialInput(Keys key)
        {
            // KeyboardDispatcher is not consistent about which "special" keys it dispatches, depending on the platform.
            // It's better not to implement this, and instead set up a separate (direct) subscription.
        }

        public void RecieveTextInput(char inputChar)
        {
            if (Selected)
            {
                owner.Insert(inputChar);
            }
        }

        public void RecieveTextInput(string text)
        {
            if (Selected)
            {
                owner.Insert(text);
            }
        }

        public void ReleaseCapture()
        {
            owner.Release();
        }

        private void KeyboardInput_KeyDown(object sender, KeyEventArgs e)
        {
            owner.HandleSpecialKey(e.KeyCode);
        }

        private void Window_KeyDown(object? sender, InputKeyEventArgs e)
        {
            owner.HandleSpecialKey(e.Key);
        }

        // Same logic used in KeyboardDispatcher.
        private static bool PlatformUsesWindowEvents()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                || Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }
}
