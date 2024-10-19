using Newtonsoft.Json;

namespace StardewUI.Framework.Diagnostics;

/// <summary>
/// Format of a trace file compatible with speedscope.
/// </summary>
/// <seealso href="https://github.com/jlfwong/speedscope"/>
public class TraceFile
{
    /// <summary>
    /// Exact date and time when the trace was started.
    /// </summary>
    [JsonIgnore]
    public DateTime CreationDate { get; } = DateTime.UtcNow;

    /// <summary>
    /// JSON schema URL; required by the Speedscope web tool.
    /// </summary>
    [JsonProperty("$schema")]
    public string SchemaUrl { get; } = "https://www.speedscope.app/file-format-schema.json";

    /// <summary>
    /// Shared trace data, containing the frames or slice names.
    /// </summary>
    public TraceShared Shared { get; } = new();

    /// <summary>
    /// List of profiles. StardewUI traces include exactly one evented profile.
    /// </summary>
    public IReadOnlyList<TraceProfile> Profiles { get; } = [new()];

    /// <summary>
    /// The name and version of the exporting mod (i.e. StardewUI).
    /// </summary>
    public string Exporter { get; init; } = "";

    /// <summary>
    /// Appends an event that closes a frame previously opened with <see cref="OpenFrame(string)"/>.
    /// </summary>
    /// <param name="frame">The index of the tracked frame in <see cref="TraceShared.Frames"/>.</param>
    public void CloseFrame(int frame)
    {
        var time = DateTime.UtcNow.Ticks / 10;
        Profiles[0].Events.Add(new('C', time, frame));
    }

    /// <summary>
    /// Adds a new <see cref="Frame"/> and <see cref="TraceEvent"/> to open it, and returns the frame index to be used
    /// subsequently with <see cref="CloseFrame(int)"/>.
    /// </summary>
    /// <param name="name">Name of the method or operation being traced.</param>
    /// <returns>The frame index, to be used with <see cref="CloseFrame(int)"/> when the operation completes.</returns>
    public int OpenFrame(string name)
    {
        var time = DateTime.UtcNow.Ticks / 10;
        int frameIndex = Shared.Frames.Count;
        Shared.Frames.Add(new(name));
        Profiles[0].Events.Add(new('O', time, frameIndex));
        return frameIndex;
    }
}

/// <summary>
/// Data shared between profiles.
/// </summary>
/// <remarks>
/// StardewUI only uses a single profile, but this structure is required by speedscope.
/// </remarks>
public class TraceShared
{
    /// <summary>
    /// The captured frames, or slices.
    /// </summary>
    public List<Frame> Frames { get; } = [];
}

/// <summary>
/// Represents a single captured frame, or slice.
/// </summary>
/// <param name="Name">Name of the method or operation that was measured.</param>
public record Frame(string Name);

/// <summary>
/// A single profile in a trace.
/// </summary>
/// <remarks>
/// For speedscope purposes, this is always an "EventedProfile". StardewUI does not use sampled profiles.
/// </remarks>
public class TraceProfile
{
    /// <summary>
    /// Discriminator for the profile type. In StardewUI, this is always <c>evented</c>.
    /// </summary>
    public string Type { get; } = "evented";

    /// <summary>
    /// Name of the profile.
    /// </summary>
    /// <remarks>
    /// This is an arbitrary string often used to indicate the name of the profile "file" that was used to configure the
    /// trace. Since StardewUI only uses a single, hardcoded "profile", this is the literal string <c>StardewUI</c>.
    /// </remarks>
    public string Name { get; } = "StardewUI";

    /// <summary>
    /// Unit of measurement for all time values.
    /// </summary>
    /// <remarks>
    /// StardewUI's traces are always measured in <c>microseconds</c>.
    /// </remarks>
    public string Unit { get; } = "microseconds";

    /// <summary>
    /// The timestamp when tracing was started, in the specified <see cref="Unit"/>.
    /// </summary>
    public long StartValue { get; } = DateTime.UtcNow.Ticks / 10;

    /// <summary>
    /// The timestamp when tracing ended, in the specified <see cref="Unit"/>.
    /// </summary>
    public long EndValue { get; }

    /// <summary>
    /// The events recorded for this profile.
    /// </summary>
    public List<TraceEvent> Events { get; } = [];
}

/// <summary>
/// Defines a single trace event.
/// </summary>
/// <remarks>
/// StardewUI uses only evented profiles, so the data is either for an <c>OpenFrameEvent</c> or <c>CloseFrameEvent</c>.
/// </remarks>
/// <param name="Type">Discriminator for the event type; <c>'O'</c> for open frame or <c>'C'</c> for close.</param>
/// <param name="At">Time when the event was logged, in the profile's <see cref="TraceProfile.Unit"/> (i.e. in
/// microseconds for any StardewUI event).</param>
/// <param name="Frame">Index into the <see cref="TraceShared.Frames"/> identifying which frame this event refers to.
/// Used to correlate open and close events.</param>
public record TraceEvent(char Type, long At, int Frame);
