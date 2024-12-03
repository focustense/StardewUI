using System.Runtime.CompilerServices;
using StardewModdingAPI;
using StardewModdingAPI.Framework.Logging;
using Xunit.Abstractions;

namespace StardewUI.Framework.Tests;

internal class TestMonitor(ITestOutputHelper output) : IMonitor
{
    public bool IsVerbose => false;

    public void Log(string message, LogLevel level = LogLevel.Trace)
    {
        try
        {
            output.WriteLine($"[{level}] {message}");
        }
        catch (InvalidOperationException)
        {
            // Ignore. This can happen solely due to test parallelism and the result of the global logger pointing to a
            // test output helper that is no longer active. This is not actually a problem in game (Monitor is thread
            // safe) and is an unhelpful reason to fail a test. Although the underlying issue can still cause test
            // output to be incorrect - i.e. to show the output for a different test case - this can be easily worked
            // around by only running the one failing test in isolation.
        }
    }

    public void LogOnce(string message, LogLevel level = LogLevel.Trace)
    {
        Log(message, level);
    }

    public void VerboseLog(string message)
    {
        Log(message);
    }

    public void VerboseLog([InterpolatedStringHandlerArgument("")] ref VerboseLogStringHandler message)
    {
        Log(message.ToString());
    }
}
