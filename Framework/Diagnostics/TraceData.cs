using System.Collections.Concurrent;
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
    /// List of profiles; each profile corresponds to a running thread.
    /// </summary>
    public List<TraceProfile> Profiles { get; } = [];

    /// <summary>
    /// The name and version of the exporting mod (i.e. StardewUI).
    /// </summary>
    public string Exporter { get; init; } = "";

    private readonly ConcurrentDictionary<string, int> frameCache = [];
    private readonly long startTime = DateTime.UtcNow.Ticks / 10;
    private readonly ConcurrentDictionary<int, int> threadProfileIndices = [];

    /// <summary>
    /// Appends an event that closes a frame previously opened with <see cref="OpenFrame(string)"/>.
    /// </summary>
    /// <param name="frame">The index of the tracked frame in <see cref="TraceShared.Frames"/>.</param>
    public void CloseFrame(int frame)
    {
        var time = DateTime.UtcNow.Ticks / 10;
        var profile = GetCurrentThreadProfile();
        profile.Events.Add(new('C', time, frame));
    }

    /// <summary>
    /// Adds a new <see cref="TraceFrame"/> and <see cref="TraceEvent"/> to open it, and returns the frame index to be used
    /// subsequently with <see cref="CloseFrame(int)"/>.
    /// </summary>
    /// <param name="name">Name of the method or operation being traced.</param>
    /// <returns>The frame index, to be used with <see cref="CloseFrame(int)"/> when the operation completes.</returns>
    public int OpenFrame(string name)
    {
        var time = DateTime.UtcNow.Ticks / 10;
        // Reusing frames will definitely slow down the tracing itself, but also massively cuts down on the trace size
        // when accumulating identical method calls over thousands of frames.
        int frameIndex = frameCache.GetOrAdd(
            name,
            _ =>
            {
                lock (Shared.Frames)
                {
                    Shared.Frames.Add(new(name));
                    return Shared.Frames.Count - 1;
                }
            }
        );
        var profile = GetCurrentThreadProfile();
        profile.Events.Add(new('O', time, frameIndex));
        return frameIndex;
    }

    private TraceProfile GetCurrentThreadProfile()
    {
        int threadId = Environment.CurrentManagedThreadId;
        var profileIndex = threadProfileIndices.GetOrAdd(
            threadId,
            _ =>
            {
                var thread = Thread.CurrentThread;
                string threadName =
                    threadId == 1 ? "Main"
                    : !thread.IsThreadPoolThread && !string.IsNullOrEmpty(thread.Name) ? thread.Name
                    : threadId.ToString();
                if (string.IsNullOrEmpty(threadName))
                {
                    threadName = threadId.ToString();
                }
                lock (Profiles)
                {
                    Profiles.Add(new(threadName, startTime));
                    return Profiles.Count - 1;
                }
            }
        );
        lock (Profiles)
        {
            return Profiles[profileIndex];
        }
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
    public List<TraceFrame> Frames { get; } = [];
}

/// <summary>
/// Represents a single captured frame, or slice.
/// </summary>
/// <param name="Name">Name of the method or operation that was measured.</param>
public record TraceFrame(string Name);

/// <summary>
/// A single profile in a trace.
/// </summary>
/// <remarks>
/// For speedscope purposes, this is always an "EventedProfile". StardewUI does not use sampled profiles.
/// </remarks>
/// <param name="name">The <see cref="Name"/> of the profile, used to identify the thread.</param>
/// <param name="startValue">The timestamp when tracing was started, in the specified <see cref="Unit"/> (default:
/// microseconds).</param>
public class TraceProfile(string name, long startValue)
{
    /// <summary>
    /// Discriminator for the profile type. In StardewUI, this is always <c>evented</c>.
    /// </summary>
    public string Type { get; } = "evented";

    /// <summary>
    /// Name of the profile. Used to identify the thread.
    /// </summary>
    public string Name { get; set; } = name;

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
    public long StartValue { get; init; } = startValue;

    /// <summary>
    /// The timestamp when tracing ended, in the specified <see cref="Unit"/>.
    /// </summary>
    public long EndValue { get; set; }

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
