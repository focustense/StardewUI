using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewUI.Events;
using StardewUI.Graphics;
using StardewUI.Layout;
using StardewUI.Overlays;
using StardewValley;

namespace StardewUI.Widgets;

/// <summary>
/// Editor widget for a <see cref="NineGridPlacement"/>, which brings up a <see cref="PositioningOverlay"/> on click.
/// </summary>
/// <remarks>
/// Appears as a grid with 9 squares, with the "selected" cell according to <see cref="Placement"/> drawn with a
/// different background color. The grid is uniform and the size of each cell is determined by the widget's actual size
/// (by way of its <see cref="IView.Layout"/>) and the current <see cref="LineWidth"/>.
/// </remarks>
public class NineGridPlacementEditor : View
{
    /// <summary>
    /// Map of buttons to button prompt sprites.
    /// </summary>
    public ISpriteMap<SButton>? ButtonSpriteMap { get; set; }

    /// <summary>
    /// The default color to draw grid cells (inside the gridlines).
    /// </summary>
    public Color CellColor
    {
        get => cellColor;
        set
        {
            if (value != cellColor)
            {
                cellColor = value;
                OnPropertyChanged(nameof(CellColor));
            }
        }
    }

    /// <summary>
    /// The content to display in the <see cref="PositioningOverlay"/> when editing. Not shown in the editor itself.
    /// </summary>
    public IView? Content
    {
        get => content;
        set
        {
            if (value != content)
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    /// <summary>
    /// Map of directions to directional arrow sprites; used to indicate dragging.
    /// </summary>
    public ISpriteMap<Direction>? DirectionSpriteMap { get; set; }

    /// <summary>
    /// The color to draw gridlines.
    /// </summary>
    public Color GridColor
    {
        get => gridColor;
        set
        {
            if (value != gridColor)
            {
                gridColor = value;
                OnPropertyChanged(nameof(GridColor));
            }
        }
    }

    /// <summary>
    /// Color to tint cells and gridlines while the mouse is hovering over the editor.
    /// </summary>
    public Color HoverTintColor
    {
        get => hoverTintColor;
        set
        {
            if (value != hoverTintColor)
            {
                hoverTintColor = value;
                OnPropertyChanged(nameof(HoverTintColor));
            }
        }
    }

    /// <summary>
    /// Thickness of gridlines.
    /// </summary>
    public int LineWidth
    {
        get => lineWidth.Value;
        set
        {
            if (lineWidth.SetIfChanged(value))
            {
                OnPropertyChanged(nameof(LineWidth));
            }
        }
    }

    /// <summary>
    /// The current content placement.
    /// </summary>
    public NineGridPlacement Placement
    {
        get => placement;
        set
        {
            if (value != placement)
            {
                placement = value;
                OnPropertyChanged(nameof(Placement));
            }
        }
    }

    /// <summary>
    /// Color for the grid cell that is is selected according to <see cref="Placement"/>.
    /// </summary>
    public Color SelectionBackgroundColor
    {
        get => selectionBackgroundColor;
        set
        {
            if (value != selectionBackgroundColor)
            {
                selectionBackgroundColor = value;
                OnPropertyChanged(nameof(SelectionBackgroundColor));
            }
        }
    }

    private readonly Rectangle[,] cells = new Rectangle[3, 3];
    private readonly Rectangle[] gridlines = new Rectangle[8];
    private readonly DirtyTracker<int> lineWidth = new(4);

    private Color cellColor = Color.Transparent;
    private IView? content;
    private Color gridColor = new(64, 64, 64);
    private Color hoverTintColor = Color.White;
    private bool isHovering;
    private PositioningOverlay? overlay;
    private NineGridPlacement placement = new(Alignment.Start, Alignment.Start);
    private Color selectionBackgroundColor = new(80, 150, 220);

    /// <inheritdoc />
    public override void OnClick(ClickEventArgs e)
    {
        if (e.IsPrimaryButton())
        {
            Game1.playSound("bigSelect");
            overlay = new PositioningOverlay(ButtonSpriteMap, DirectionSpriteMap)
            {
                Content = Content,
                ContentPlacement = Placement,
                DimmingAmount = 0.93f,
            };
            overlay.Close += Overlay_Close;
            Overlay.Push(overlay);
            e.Handled = true;
        }
        base.OnClick(e);
    }

    /// <inheritdoc />
    public override void OnPointerMove(PointerMoveEventArgs e)
    {
        isHovering = ContainsPoint(e.Position);
        base.OnPointerMove(e);
    }

    /// <inheritdoc />
    protected override bool IsContentDirty()
    {
        return lineWidth.IsDirty;
    }

    /// <inheritdoc />
    protected override void OnDrawContent(ISpriteBatch b)
    {
        var tintedSelectedCellColor = Tint(SelectionBackgroundColor);
        var tintedCellColor = Tint(CellColor);
        for (int row = 0; row < cells.GetLength(0); row++)
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                var color = IsSelectedCell(col, row) ? tintedSelectedCellColor : tintedCellColor;
                if (color != Color.Transparent)
                {
                    b.Draw(Game1.staminaRect, cells[row, col], null, color);
                }
            }
        }
        if (GridColor == Color.Transparent)
        {
            return;
        }
        var tintedGridColor = Tint(GridColor);
        foreach (var gridline in gridlines)
        {
            b.Draw(Game1.staminaRect, gridline, null, Tint(tintedGridColor));
        }
    }

    /// <inheritdoc />
    protected override void OnMeasure(Vector2 availableSize)
    {
        var limits = Layout.GetLimits(availableSize);
        ContentSize = Layout.Resolve(availableSize, () => Vector2.Zero);

        int columnWidth = (int)MathF.Round((ContentSize.X - LineWidth * 4) / 3);
        int rowHeight = (int)MathF.Round((ContentSize.Y - LineWidth * 4) / 3);
        // Cells
        int y = 0;
        for (int row = 0; row < 3; row++)
        {
            y += LineWidth;
            int x = 0;
            for (int col = 0; col < 3; col++)
            {
                x += LineWidth;
                cells[row, col] = new(x, y, columnWidth, rowHeight);
                x += columnWidth;
            }
            y += rowHeight;
        }
        // Gridlines
        int gridWidth = columnWidth * 3 + LineWidth * 4;
        int gridHeight = rowHeight * 3 + LineWidth * 4;
        int gridX = 0;
        for (int col = 0; col < 4; col++)
        {
            gridlines[col] = new(gridX, 0, LineWidth, gridHeight);
            gridX += LineWidth + columnWidth;
        }
        int gridY = 0;
        for (int row = 0; row < 4; row++)
        {
            gridlines[row + 4] = new(0, gridY, gridWidth, LineWidth);
            gridY += LineWidth + rowHeight;
        }
    }

    /// <inheritdoc />
    protected override void ResetDirty()
    {
        lineWidth.ResetDirty();
    }

    private static int GetCellIndex(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.Start => 0,
            Alignment.Middle => 1,
            Alignment.End => 2,
            _ => throw new ArgumentException($"Invalid alignment: {alignment}", nameof(alignment)),
        };
    }

    private bool IsSelectedCell(int col, int row)
    {
        return col == GetCellIndex(Placement.HorizontalAlignment) && row == GetCellIndex(Placement.VerticalAlignment);
    }

    private void Overlay_Close(object? sender, EventArgs e)
    {
        if (overlay is not null)
        {
            Placement = overlay.ContentPlacement;
        }
        overlay = null;
        isHovering = false;
    }

    private Color Tint(Color color)
    {
        if (!isHovering || HoverTintColor.A == 0 || HoverTintColor == Color.White)
        {
            return color;
        }
        const float tintAmount = 0.25f;
        float tintAlpha = 255 / HoverTintColor.A;
        int r = (int)(color.R * (1 - tintAmount) + HoverTintColor.R * tintAmount * tintAlpha);
        int g = (int)(color.G * (1 - tintAmount) + HoverTintColor.G * tintAmount * tintAlpha);
        int b = (int)(color.B * (1 - tintAmount) + HoverTintColor.B * tintAmount * tintAlpha);
        int a = (int)(color.A * (1 - tintAmount) + HoverTintColor.A * tintAmount);
        return new(r, g, b, a);
    }
}
