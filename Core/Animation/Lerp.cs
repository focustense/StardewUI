namespace StardewUI.Animation;

/// <summary>
/// Performs linear interpolation between two values.
/// </summary>
/// <typeparam name="T">The type of value.</typeparam>
/// <param name="value1">The first, or "start" value to use at <paramref name="amount"/> = <c>0.0</c>.</param>
/// <param name="value2">The second, or "end" value to use at <paramref name="amount"/> = <c>1.0</c>.</param>
/// <param name="amount">The interpolation amount between <c>0.0</c> and <c>1.0</c>.</param>
/// <returns>The interpolated value.</returns>
public delegate T Lerp<T>(T value1, T value2, float amount);
