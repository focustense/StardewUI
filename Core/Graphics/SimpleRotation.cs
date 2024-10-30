namespace StardewUI.Graphics;

/// <summary>
/// Types of rotations that are considered to be "simple", i.e. those that only transpose pixels and are therefore fast
/// and non-deforming.
/// </summary>
public enum SimpleRotation
{
    /// <summary>
    /// Rotate 90° in the clockwise direction.
    /// </summary>
    QuarterClockwise,

    /// <summary>
    /// Rotate 90° in the counterclockwise direction.
    /// </summary>
    QuarterCounterclockwise,

    /// <summary>
    /// Rotate 180°.
    /// </summary>
    Half,
}

/// <summary>
/// Helper extensions for the <see cref="SimpleRotation"/> type.
/// </summary>
public static class SimpleRotationExtensions
{
    private const float HALF_PI = MathF.PI / 2;

    /// <summary>
    /// Gets the angle of a rotation, in radians.
    /// </summary>
    /// <param name="rotation">The rotation type.</param>
    /// <returns>The angle of the rotation, in radians.</returns>
    public static float Angle(this SimpleRotation rotation)
    {
        return rotation switch
        {
            SimpleRotation.QuarterClockwise => HALF_PI,
            SimpleRotation.QuarterCounterclockwise => -HALF_PI,
            SimpleRotation.Half => MathF.PI,
            _ => 0,
        };
    }

    /// <summary>
    /// Gets whether a rotation is a quarter turn.
    /// </summary>
    /// <remarks>
    /// Often used to check whether to invert X/Y values in measurements.
    /// </remarks>
    /// <param name="rotation">The rotation type.</param>
    /// <returns><c>true</c> if the current instance is one of <see cref="SimpleRotation.QuarterClockwise"/> or
    /// <see cref="SimpleRotation.QuarterCounterclockwise"/>; otherwise <c>false</c>.</returns>
    public static bool IsQuarter(this SimpleRotation rotation)
    {
        return rotation == SimpleRotation.QuarterClockwise || rotation == SimpleRotation.QuarterCounterclockwise;
    }
}
