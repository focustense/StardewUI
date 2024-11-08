using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Xna.Framework;

namespace StardewUI.Layout;

/// <summary>
/// Layout parameters for an <see cref="IView"/>.
/// </summary>
[DuckType]
public readonly struct LayoutParameters : IEquatable<LayoutParameters>
{
    /// <summary>
    /// Creates a <see cref="LayoutParameters"/> that stretches to the available horizontal width, fits the content
    /// height, and has no other constraints. Typically used for rows in a vertical layout.
    /// </summary>
    /// <returns></returns>
    public static LayoutParameters AutoRow()
    {
        return new() { Width = Length.Stretch(), Height = Length.Content() };
    }

    /// <summary>
    /// Creates a <see cref="LayoutParameters"/> that stretches to the full available width and height.
    /// </summary>
    /// <returns></returns>
    public static LayoutParameters Fill()
    {
        return new() { Width = Length.Stretch(), Height = Length.Stretch() };
    }

    /// <summary>
    /// Creates a <see cref="LayoutParameters"/> that tracks content width and height, and has no other constraints.
    /// </summary>
    public static LayoutParameters FitContent()
    {
        return new() { Width = Length.Content(), Height = Length.Content() };
    }

    /// <summary>
    /// Creates a <see cref="LayoutParameters"/> with fixed dimensions, and no other constraints.
    /// </summary>
    /// <param name="size">The layout size, in pixels.</param>
    public static LayoutParameters FixedSize(Point size)
    {
        return FixedSize(size.X, size.Y);
    }

    /// <summary>
    /// Creates a <see cref="LayoutParameters"/> with fixed dimensions, and no other constraints.
    /// </summary>
    /// <param name="width">The layout width, in pixels.</param>
    /// <param name="height">The layout height, in pixels.</param>
    public static LayoutParameters FixedSize(float width, float height)
    {
        return new() { Width = Length.Px(width), Height = Length.Px(height) };
    }

    /// <summary>
    /// Initializes a new <see cref="LayoutParameters"/> with default layout settings.
    /// </summary>
    public LayoutParameters()
    {
        Width = Length.Content();
        Height = Length.Content();
    }

    /// <summary>
    /// The horizontal width/layout method.
    /// </summary>
    public Length Width { get; init; }

    /// <summary>
    /// The vertical height/layout method.
    /// </summary>
    public Length Height { get; init; }

    /// <summary>
    /// Maximum width allowed.
    /// </summary>
    /// <remarks>
    /// If specified, the <see cref="Vector2.X"/> component of a view's content size should never exceed this value,
    /// regardless of how the <see cref="Width"/> is configured.
    /// </remarks>
    public float? MaxWidth { get; init; }

    /// <summary>
    /// Maximum height allowed.
    /// </summary>
    /// <remarks>
    /// If specified, the <see cref="Vector2.Y"/> component of a view's content size should never exceed this value,
    /// regardless of how the <see cref="Height"/> is configured.
    /// </remarks>
    public float? MaxHeight { get; init; }

    /// <summary>
    /// Minimum width to occupy.
    /// </summary>
    /// <remarks>
    /// If specified, the <see cref="Vector2.X"/> component of a view's content size will always be at least this value,
    /// regardless of how the <see cref="Width"/> is configured. Typically, minimum sizes are only used with
    /// <see cref="LengthType.Content"/> if there might be very little content. If a <see cref="MaxWidth"/> is also
    /// specified and is smaller than the <c>MinWidth</c>, then <c>MaxWidth</c> takes precedence.
    /// </remarks>
    public float? MinWidth { get; init; }

    /// <summary>
    /// Minimum height to occupy.
    /// </summary>
    /// <remarks>
    /// If specified, the <see cref="Vector2.Y"/> component of a view's content size will always be at least this value,
    /// regardless of how the <see cref="Height"/> is configured. Typically, minimum sizes are only used with
    /// <see cref="LengthType.Content"/> if there might be very little content. If a <see cref="MaxHeight"/> is also
    /// specified and is smaller than the <c>MinHeight</c>, then <c>MaxHeight</c> takes precedence.
    /// </remarks>
    public float? MinHeight { get; init; }

    /// <inheritdoc/>
    /// <remarks>
    /// Overrides the default implementation to avoid using reflection on every frame during dirty checks.
    /// </remarks>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is LayoutParameters other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(LayoutParameters other)
    {
        return Width == other.Width
            && Height == other.Height
            && MaxWidth == other.MaxWidth
            && MaxHeight == other.MaxHeight
            && MinWidth == other.MinWidth
            && MinHeight == other.MinHeight;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Width);
        hash.Add(Height);
        hash.Add(MaxWidth);
        hash.Add(MaxHeight);
        hash.Add(MinWidth);
        hash.Add(MinHeight);
        return hash.ToHashCode();
    }

    /// <summary>
    /// Determines the effective content size limits.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Limits are not the same as the actual size coming from a <see cref="Resolve"/>; they provide a maximum width
    /// and/or height in the event that one or both dimensions are set to <see cref="LengthType.Content"/>. In these
    /// cases, the caller usually wants the "constraints" - e.g. a text block with fixed width but variable height needs
    /// to know that width before it can determine the actual height.
    /// </para>
    /// <para>
    /// Implementations of <see cref="View"/> will typically obtain the limits in their <see cref="View.OnMeasure"/>
    ///  method in order to perform internal/child layout, and determine the content size for <see cref="Resolve"/>.
    /// </para>
    /// </remarks>
    /// <param name="availableSize">The available size in the container/parent.</param>
    /// <returns>The size (equal to or smaller than <paramref name="availableSize"/>) that can be allocated to
    /// content.</returns>
    public Vector2 GetLimits(Vector2 availableSize)
    {
        // These constraints are given to the getContentSize selector IF they are known, i.e. if those dimensions do not
        // circularly depend on the content size. In other words, suppose an image is specified to have a fixed width,
        // but have its height match the content; here we will get a valid value for constrainedWidth and an invalid
        // value for constrainedHeight, which will be translated into a constrained size having the computed width and
        // original (container-available) height.
        var constrainedWidth = Math.Min(
            // Despite the odd, seemingly-redundant look of this code, it's doing the correct thing:
            // - Always constrain to the max width/height, no matter what else happens;
            // - If the length is not content-dependent, then use it;
            // - Otherwise, set the limit to the maximum size available (which is subsequently constrained by max).
            Width.Resolve(availableSize.X, () => availableSize.X),
            MaxWidth ?? float.PositiveInfinity
        );
        var constrainedHeight = Math.Min(
            Height.Resolve(availableSize.Y, () => availableSize.Y),
            MaxHeight ?? float.PositiveInfinity
        );
        return new Vector2(constrainedWidth, constrainedHeight);
    }

    /// <summary>
    /// Resolves the actual size for the current <see cref="LayoutParameters"/>.
    /// </summary>
    /// <param name="availableSize">The available size in the container/parent.</param>
    /// <param name="getContentSize">Function to compute the inner content size based on limits obtained from
    /// <see cref="GetLimits"/>; will only be invoked if it is required for the current layout configuration, i.e. if
    /// one or both dimensions are set to fit content.</param>
    /// <returns></returns>
    public Vector2 Resolve(Vector2 availableSize, Func<Vector2> getContentSize)
    {
        Vector2? contentSize = null;

        float Resolve1D(Func<Vector2, float> getLength)
        {
            contentSize ??= getContentSize();
            return getLength(contentSize.Value);
        }

        var resolvedWidth = Width.Resolve(availableSize.X, () => Resolve1D(size => size.X));
        var resolvedHeight = Height.Resolve(availableSize.Y, () => Resolve1D(size => size.Y));
        return new(
            Math.Clamp(resolvedWidth, MinWidth ?? float.NegativeInfinity, MaxWidth ?? float.PositiveInfinity),
            Math.Clamp(resolvedHeight, MinHeight ?? float.NegativeInfinity, MaxHeight ?? float.PositiveInfinity)
        );
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();
        Append(Width, MinWidth, MaxWidth);
        sb.Append(' ');
        Append(Height, MinHeight, MaxHeight);
        return sb.ToString();

        void Append(Length length, float? min, float? max)
        {
            sb.Append(length);
            if (!min.HasValue && !max.HasValue)
            {
                return;
            }
            sb.Append('[');
            if (min.HasValue)
            {
                sb.Append(min);
            }
            sb.Append("..");
            if (max.HasValue)
            {
                sb.Append(max);
            }
            sb.Append(']');
        }
    }

    /// <summary>
    /// Compares two <see cref="LayoutParameters"/> values for equality.
    /// </summary>
    /// <param name="left">The first value.</param>
    /// <param name="right">The second value.</param>
    /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are the same layout, otherwise
    /// <c>false</c>.</returns>
    public static bool operator ==(LayoutParameters left, LayoutParameters right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="LayoutParameters"/> values for inequality.
    /// </summary>
    /// <param name="left">The first value.</param>
    /// <param name="right">The second value.</param>
    /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are different layouts, otherwise
    /// <c>false</c>.</returns>
    public static bool operator !=(LayoutParameters left, LayoutParameters right)
    {
        return !(left == right);
    }
}
