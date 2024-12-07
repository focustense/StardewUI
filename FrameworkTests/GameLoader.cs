using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewValley;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework("StardewUI.Framework.Tests.GameLoader", "StardewUI.Framework.Tests")]

namespace StardewUI.Framework.Tests;

internal sealed class GameLoader : XunitTestFramework, IDisposable
{
    private readonly GameRunner game;
    private readonly GameSettings gameSettings;

    public GameLoader(IMessageSink messageSink)
        : base(messageSink)
    {
        gameSettings =
            JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText("GameSettings.json"))
            ?? throw new InvalidOperationException(
                "Couldn't load game settings generated from the StardewUI Test's MSBuild task"
            );
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        Environment.SetEnvironmentVariable(
            "PATH",
            Environment.GetEnvironmentVariable("PATH") + ";" + gameSettings.GamePath
        );
        if (GetContentDirectory(gameSettings.GamePath) is not { } contentDirectory)
        {
            throw new InvalidOperationException(
                $"Couldn't find valid content path for game path {gameSettings.GamePath}."
            );
        }
        game = new GameRunner();
        game.Content.RootDirectory = contentDirectory;
        GameRunner.instance = game;
        Game1.graphics.GraphicsProfile = GraphicsProfile.Reach;
        game.RunOneFrame();
    }

    public new void Dispose()
    {
        base.Dispose();
        game.Dispose();
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        GC.SuppressFinalize(this);
    }

    private static string? CheckPath(string path)
    {
        var directory = new DirectoryInfo(path);
        return directory.Exists ? directory.FullName : null;
    }

    private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        if (AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name) is { } loaded)
        {
            return loaded;
        }
        string fileName = args.Name.Split(',')[0] + ".dll";
        if (
            FileExistsIgnoreCase(gameSettings.GamePath, fileName, out var file)
            || FileExistsIgnoreCase(Path.Combine(gameSettings.GamePath, "smapi-internal"), fileName, out file)
        )
        {
            return Assembly.LoadFrom(file.FullName);
        }
        return null;
    }

    private static bool FileExistsIgnoreCase(
        string directoryPath,
        string fileName,
        [MaybeNullWhen(false)] out FileInfo file
    )
    {
        file = new DirectoryInfo(directoryPath)
            .GetFiles(fileName, new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive })
            .FirstOrDefault();
        return file is not null;
    }

    private static string? GetContentDirectory(string gamePath)
    {
        return Constants.TargetPlatform switch
        {
            GamePlatform.Mac => CheckPath(Path.Combine(gamePath, "../Resources/Content"))
                ?? CheckPath(Path.Combine(gamePath, "../../Resources/Content")),
            _ => CheckPath(Path.Combine(gamePath, "Content")),
        };
    }
}
