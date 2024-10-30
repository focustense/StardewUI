using StardewUI.Layout;

namespace StardewUI.Widgets;

/// <summary>
/// Fluent builder style API for creating form-like tables within a view.
/// </summary>
/// <remarks>
/// Useful for "settings" style views.
/// </remarks>
/// <param name="labelWidth">The width of the label column. Same for all rows.</param>
/// <param name="fieldIndent">Pixel amount by which to indent the field rows, relative to any heading rows.</param>
public class FormBuilder(int labelWidth, int fieldIndent = 16)
{
    private const int FIELD_SPACING = 8;

    private readonly List<IView> rows = [];

    private Edges margin = Edges.NONE;
    private Edges padding = Edges.NONE;

    /// <summary>
    /// Builds the form.
    /// </summary>
    /// <returns>The view containing the form layout.</returns>
    public IView Build()
    {
        return new Lane()
        {
            Name = "Form",
            Layout = LayoutParameters.AutoRow(),
            Margin = margin,
            Padding = padding,
            Orientation = Orientation.Vertical,
            Children = rows,
        };
    }

    /// <summary>
    /// Adds a custom row, which is simply a horizontal <see cref="Lane"/> consisting of the specified views, not
    /// including any label - i.e. the first view is flush with the labels on other rows.
    /// </summary>
    /// <remarks>
    /// Might be used for a row of confirmation buttons, a paragraph of help text, etc.
    /// </remarks>
    /// <param name="views">The views to display in this row.</param>
    /// <returns>The current builder instance.</returns>
    public FormBuilder AddCustom(params IView[] views)
    {
        var row = new Lane()
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = new(Left: fieldIndent, Top: FIELD_SPACING, Bottom: FIELD_SPACING),
            VerticalContentAlignment = Alignment.Middle,
            ScrollWithChildren = Orientation.Vertical,
            Children = views,
        };
        rows.Add(row);
        return this;
    }

    /// <summary>
    /// Adds a labeled control row, i.e. a field.
    /// </summary>
    /// <param name="title">The field title, displayed on the left side.</param>
    /// <param name="description">Description of the field's value or purpose, displayed as a tooltip when hovering over
    /// the title text.</param>
    /// <param name="view">The view for modifying the field's value.</param>
    /// <returns>The current builder instance.</returns>
    public FormBuilder AddField(string title, string? description = null, IView? view = null)
    {
        var label = new Label()
        {
            Layout = new() { Width = Length.Px(labelWidth), Height = Length.Content() },
            Text = title,
            Tooltip = description ?? "",
        };
        var row = new Lane()
        {
            Layout = LayoutParameters.AutoRow(),
            Margin = new(Left: fieldIndent, Top: FIELD_SPACING, Bottom: FIELD_SPACING),
            VerticalContentAlignment = Alignment.Middle,
            ScrollWithChildren = Orientation.Vertical,
            Children = view is not null ? [label, view] : [label],
        };
        rows.Add(row);
        return this;
    }

    /// <summary>
    /// Starts a new section by adding header text.
    /// </summary>
    /// <param name="title">The section title.</param>
    /// <returns>The current builder instance.</returns>
    public FormBuilder AddSection(string title)
    {
        var heading = CreateSectionHeading(title);
        rows.Add(heading);
        return this;
    }

    /// <summary>
    /// Creates the banner used as a section heading.
    /// </summary>
    /// <remarks>
    /// This is the standalone version of <see cref="AddSection(string)"/> that can be used by any views wanting to
    /// provide form-style section headings outside of an actual form.
    /// </remarks>
    /// <param name="title">The section title.</param>
    /// <returns>Section heading view.</returns>
    public static IView CreateSectionHeading(string title)
    {
        return new Banner() { Margin = new(Top: FIELD_SPACING * 2), Text = title };
    }

    /// <summary>
    /// Configures the margin for the entire form.
    /// </summary>
    /// <param name="margin">Margin outside the form.</param>
    /// <returns>The current builder instance.</returns>
    public FormBuilder SetMargin(Edges margin)
    {
        this.margin = margin;
        return this;
    }

    /// <summary>
    /// Configures the padding for the entire form.
    /// </summary>
    /// <param name="padding">Padding inside the form.</param>
    /// <returns>The current builder instance.</returns>
    public FormBuilder SetPadding(Edges padding)
    {
        this.padding = padding;
        return this;
    }
}
