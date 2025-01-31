using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Xna.Framework.Content;
using StardewUI.Framework.Content;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// Utility for loading <see cref="Document"/>s from memory or files.
/// </summary>
public static class DocumentLoader
{
    /// <summary>
    /// Loads a <see cref="Document"/> from markup in a file.
    /// </summary>
    /// <remarks>
    /// This method is designed to be called from SMAPI's content loader, and throws exceptions normally associated with
    /// SMAPI's content pipeline.
    /// </remarks>
    /// <param name="file">The file containing the document markup.</param>
    /// <returns>The parsed <see cref="Document"/>.</returns>
    /// <exception cref="ContentLoadException">Thrown when the document could not be parsed. The details of the
    /// exception are logged to the SMAPI console before throwing.</exception>
    public static Document LoadFromFile(FileInfo file)
    {
        using var _ = Trace.Begin(nameof(DocumentLoader), nameof(LoadFromFile));
        string markup = File.ReadAllText(file.FullName);
        return TryParse(file.FullName, markup, out var document)
            ? document
            : throw new ContentLoadException($"Failed loading StarML Document from file: {file.FullName}");
    }

    /// <summary>
    /// Loads a <see cref="Document"/> from markup in a file using asynchronous I/O.
    /// </summary>
    /// <param name="file">The file containing the document markup.</param>
    /// <returns>The parsed <see cref="Document"/>, or <c>null</c> if the file does not exist or the markup contained
    /// in the file is invalid.</returns>
    public static async Task<Document?> TryLoadFromFileAsync(FileInfo file)
    {
        using var _ = Trace.Begin(nameof(DocumentLoader), nameof(TryLoadFromFileAsync));
        if (!file.Exists)
        {
            return null;
        }
        string markup = await File.ReadAllTextAsync(file.FullName);
        return TryParse(file.FullName, markup, out var document) ? document : null;
    }

    private static bool TryParse(string originalPath, string markup, [MaybeNullWhen(false)] out Document document)
    {
        try
        {
            document = Document.Parse(markup);
            SourceResolver.SetDocumentSourcePath(document, originalPath);
            return true;
        }
        catch (LexerException ex)
        {
            LogMarkupError(ex, Path.GetFileName(originalPath), markup, ex.Position);
            document = null;
            return false;
        }
        catch (ParserException ex)
        {
            LogMarkupError(ex, Path.GetFileName(originalPath), markup, ex.Position);
            document = null;
            return false;
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
