// Misc types used for low-level UI conversion. Separated from view models for clarity.

namespace StardewUITest.Examples.Tempering;

internal record Transition(TimeSpan Duration, TimeSpan Delay = default);
