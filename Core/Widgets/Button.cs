﻿using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewUI;

/// <summary>
/// Simple button with optional hover background.
/// </summary>
/// <param name="defaultBackgroundSprite">The default background to show for the button's idle state.</param>
/// <param name="hoverBackgroundSprite">Alternate background sprite when the button has cursor focus.</param>
public class Button(Sprite? defaultBackgroundSprite = null, Sprite? hoverBackgroundSprite = null) : ComponentView<View>
{
    /// <summary>
    /// Content view to display inside the button frame.
    /// </summary>
    public IView? Content
    {
        get => contentFrame.Content;
        set
        {
            if (value != contentFrame.Content)
            {
                contentFrame.Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    /// <summary>
    /// Font with which to render button text.
    /// </summary>
    /// <remarks>
    /// This setting only applies when the <see cref="Content"/> view is a <see cref="Label"/>, either via passing in
    /// a <see cref="Label"/> directly or by setting <see cref="Text"/>.
    /// </remarks>
    public SpriteFont Font
    {
        get => font;
        set
        {
            if (value == font)
            {
                return;
            }
            font = value;
            if (contentFrame.Content is Label label)
            {
                label.Font = font;
            }
            OnPropertyChanged(nameof(Font));
        }
    }

    /// <summary>
    /// Margin to add outside the button.
    /// </summary>
    public Edges Margin
    {
        get => View.Margin;
        set => View.Margin = value;
    }

    /// <summary>
    /// Whether or not to display a drop shadow for the button frame. Default <c>false</c>.
    /// </summary>
    public bool ShadowVisible
    {
        get => backgroundImage.ShadowAlpha > 0;
        set
        {
            if (value != backgroundImage.ShadowAlpha > 0)
            {
                backgroundImage.ShadowAlpha = value ? 0.5f : 0f;
                OnPropertyChanged(nameof(ShadowVisible));
            }
        }
    }

    /// <summary>
    /// Text to display inside the button.
    /// </summary>
    /// <remarks>
    /// If the <see cref="Content"/> is not a <see cref="Label"/> then this is always <c>null</c>, even if there is a
    /// label nested somewhere inside a different type of view. Setting this to any string value will <b>replace</b> the
    /// <see cref="Content"/> view with a <see cref="Label"/> having the specified text.
    /// </remarks>
    public string? Text
    {
        get => contentFrame.Content is Label label ? label.Text : null;
        set
        {
            if (contentFrame.Content is Label label)
            {
                if ((value ?? "") != label.Text)
                {
                    label.Text = value ?? "";
                    OnPropertyChanged(nameof(Text));
                }
            }
            else if (value is not null)
            {
                contentFrame.Content = Label.Simple(value, font);
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    private SpriteFont font = Game1.smallFont;

    // Initialized in CreateView
    private Image backgroundImage = null!;
    private Frame contentFrame = null!;

    /// <inheritdoc />
    protected override View CreateView()
    {
        backgroundImage = new Image()
        {
            Layout = LayoutParameters.Fill(),
            Fit = ImageFit.Stretch,
            ShadowOffset = new(-4, 4),
        };
        UpdateBackgroundImage(false);
        contentFrame = new Frame() { Layout = LayoutParameters.FitContent(), Margin = new(16, 12) };
        var panel = new Panel()
        {
            Layout = new()
            {
                Width = Length.Content(),
                Height = Length.Content(),
                MinWidth = 64,
                MinHeight = 32,
            },
            HorizontalContentAlignment = Alignment.Middle,
            VerticalContentAlignment = Alignment.Middle,
            Children = [backgroundImage, contentFrame],
            Focusable = true,
        };
        panel.PointerEnter += Panel_PointerEnter;
        panel.PointerLeave += Panel_PointerLeave;
        return panel;
    }

    private void Panel_PointerEnter(object? sender, PointerEventArgs e)
    {
        UpdateBackgroundImage(true);
    }

    private void Panel_PointerLeave(object? sender, PointerEventArgs e)
    {
        UpdateBackgroundImage(false);
    }

    private void UpdateBackgroundImage(bool hover)
    {
        if (backgroundImage is null)
        {
            return;
        }
        backgroundImage.Sprite = hover
            ? hoverBackgroundSprite ?? defaultBackgroundSprite ?? UiSprites.ButtonDark
            : defaultBackgroundSprite ?? UiSprites.ButtonDark;
    }
}
