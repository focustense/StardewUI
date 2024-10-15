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
        output.WriteLine($"[{level}] {message}");
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
