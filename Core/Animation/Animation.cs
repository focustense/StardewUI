namespace StardewUI.Animation;

/// <summary>
/// Defines a single animation.
/// </summary>
/// <typeparam name="T">The type of value being animated.</typeparam>
/// <param name="StartValue">The initial value for the animated property.</param>
/// <param name="EndValue">The final value for the animated property.</param>
/// <param name="Duration">Duration of the animation.</param>
public record Animation<T>(T StartValue, T EndValue, TimeSpan Duration);
