using System.Text;
using Microsoft.Xna.Framework.Content;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Patches;

/// <summary>
/// Patches for SMAPI's <c>ModContentManager</c> (internal).
/// </summary>
internal static class ModContentManagerPatches
{
    public static bool HandleUnknownFileType_Prefix(
        IAssetName assetName,
        FileInfo file,
        Type assetType,
        ref object __result
    )
    {
        var ext = file.Extension.ToLower();
        if (ext == ".sml" || ext == ".starml")
        {
            if (assetType != typeof(Document) && assetType != typeof(object))
            {
                throw new ContentLoadException(
                    $"Failed loading asset '{assetName}' from {file.Name}: the target asset type is incorrect "
                        + $"(expected {typeof(Document).FullName}, but got {assetType.FullName})."
                );
            }
            __result = LoadDocument(file);
            return false;
        }
        return true;
    }

    private static Document LoadDocument(FileInfo file)
    {
        string markup = File.ReadAllText(file.FullName);
        try
        {
            return Document.Parse(markup);
        }
        catch (LexerException ex)
        {
            LogMarkupError(ex, file.Name, markup, ex.Position);
            throw new ContentLoadException($"Failed loading StarML Document from file: {file.FullName}");
        }
        catch (ParserException ex)
        {
            LogMarkupError(ex, file.Name, markup, ex.Position);
            throw new ContentLoadException($"Failed loading StarML Document from file: {file.FullName}");
        }
    }

    private static void AppendErrorLines(StringBuilder sb, string text, int position)
    {
        // The parser doesn't (and shouldn't) care about lines, but in order to help track down the exact location of
        // the error, it's very useful to backtrack here and figure that out.
        var textSpan = text.AsSpan();
        ReadOnlySpan<char> errorLine;
        int lineNumber = 0;
        int lineStartPos = 0;
        do
        {
            lineNumber++;
            int nextNewlinePos = textSpan.IndexOf('\n');
            int lineLength = nextNewlinePos >= 0 ? nextNewlinePos + 1 : textSpan.Length;
            errorLine = textSpan[..lineLength];
            textSpan = textSpan[lineLength..];
            lineStartPos += lineLength;
        } while (!textSpan.IsEmpty && lineStartPos < position);

        if (errorLine.IsWhiteSpace())
        {
            return;
        }
        lineStartPos -= errorLine.Length;
        int maxLineLength = Console.BufferWidth;
        int originalPositionInLine = position - lineStartPos;
        int positionInLine = originalPositionInLine;
        int untrimmedLength = errorLine.Length;
        errorLine = errorLine.TrimStart();
        positionInLine -= (untrimmedLength - errorLine.Length);
        errorLine = errorLine.TrimEnd();
        // If line is still too long after all this, just trim non-whitespace proportionally.
        if (errorLine.Length > maxLineLength)
        {
            float positionRatio = (float)positionInLine / errorLine.Length;
            int newLengthBefore = (int)(positionRatio * maxLineLength);
            int positionBefore = positionInLine - newLengthBefore;
            errorLine = errorLine[positionBefore..(positionBefore + maxLineLength)];
            positionInLine = newLengthBefore;
        }
        sb.AppendLine();
        sb.AppendLine(errorLine.ToString());
        int spaceBefore = Math.Max(0, positionInLine - 1);
        if (spaceBefore > 0)
        {
            sb.Append(new string(' ', spaceBefore));
        }
        int caretCount = Math.Min(3, maxLineLength - 3 - spaceBefore);
        sb.Append(new string('^', caretCount));
        sb.AppendLine();
        sb.Append("  (Line ")
            .Append(lineNumber)
            .Append(", char ")
            .Append(originalPositionInLine + 1)
            .AppendLine(")")
            .AppendLine();
    }

    private static void LogMarkupError(Exception ex, string fileName, string text, int position)
    {
        var sb = new StringBuilder("Invalid markup detected in file '").Append(fileName).AppendLine("'.");
        try
        {
            AppendErrorLines(sb, text, position);
        }
        catch
        {
            // Never fail in an error logger; log whatever we're able to.
        }
        sb.Append(ex);
        Logger.Log(sb.ToString(), LogLevel.Error);
    }
}
