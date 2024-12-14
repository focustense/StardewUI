namespace StardewUI.Graphics;

/// <summary>
/// Describes the origin point to use for a local <see cref="Transform"/>.
/// </summary>
/// <remarks>
/// <para>
/// Origin data needs to track two vectors; the relative or percentage position with <see cref="Vector2.X"/> and
/// <see cref="Vector2.Y"/> between <c>0</c> and <c>1</c> (e.g. the center of the layout would be <c>(0.5, 0.5)</c>)
/// as well as the absolute or pixel position.
/// </para>
/// <para>
/// The relative position is used for individual drawing operations; when drawing a single sprite or text string, the
/// XNA drawing APIs use this exact origin vector. The absolute position, on the other hand, is required for transform
/// propagation in the <see cref="GlobalTransform"/>, i.e. if the custom-origin transform is applied to a layout view,
/// because it must be converted into a translation matrix.
/// </para>
/// </remarks>
/// <param name="Relative">The relative position with <see cref="Vector2.X"/> and <see cref="Vector2.Y"/> values between
/// <c>0</c> and <c>1</c>, where <c>(0, 0)</c> is the top-left, <c>(0.5, 0.5)</c> is the middle, and <c>(1, 1)</c> is
/// the bottom right.</param>
/// <param name="Absolute">The pixel position of the exact origin point, relative to the transformed view's top-left
/// corner.</param>
public record TransformOrigin(Vector2 Relative, Vector2 Absolute)
{
    /// <summary>
    /// Default origin, with both <see cref="Relative"/> and <see cref="Absolute"/> set to <see cref="Vector2.Zero"/>.
    /// </summary>
    public static readonly TransformOrigin Default = new(Vector2.Zero, Vector2.Zero);
}
