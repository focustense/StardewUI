using System.Collections.Immutable;
using StardewUI.Framework.Grammar;

namespace StardewUI.Framework.Dom;

/// <summary>
/// A standalone StarML document.
/// </summary>
/// <param name="Root">The primary content node.</param>
/// <param name="Templates">List of template nodes for inline expansion.</param>
public record Document(SNode Root, IReadOnlyList<SNode> Templates)
{
    /// <summary>
    /// Parses a <see cref="Document"/> from its original markup text.
    /// </summary>
    /// <param name="text">The StarML markup text.</param>
    /// <returns>The parsed document as a DOM tree.</returns>
    /// <exception cref="ParserException">Thrown when any invalid markup is encountered.</exception>
    public static Document Parse(ReadOnlySpan<char> text)
    {
        using var _ = Trace.Begin(nameof(Document), nameof(Parse));
        var reader = new DocumentReader(text);
        SNode? root = null;
        var templates = ImmutableArray.CreateBuilder<SNode>();
        while (!reader.Eof)
        {
            int position = reader.Position;
            var node = SNode.Parse(ref reader);
            if (node.Tag.Equals("template", StringComparison.OrdinalIgnoreCase))
            {
                var nameAttribute = node.Attributes.FirstOrDefault(attr =>
                    attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase)
                );
                if (nameAttribute is null)
                {
                    throw new ParserException("<template> element is missing a 'name' attribute.", position);
                }
                else if (nameAttribute.ValueType != AttributeValueType.Literal)
                {
                    throw new ParserException(
                        $"<template> element uses invalid {nameAttribute.ValueType} type for 'name' attribute. "
                            + "Template names must be literal (quoted) strings.",
                        position
                    );
                }
                else if (string.IsNullOrEmpty(nameAttribute.Value))
                {
                    throw new ParserException("<template> element has an empty 'name' attribute.", position);
                }
                templates.Add(node);
            }
            else
            {
                if (root is not null)
                {
                    throw new ParserException(
                        $"View cannot have multiple content nodes.\n\nPrevious root was: {root}",
                        position
                    );
                }
                root = node;
            }
        }
        if (root is null)
        {
            throw new ParserException("Document is missing a content node.", 0);
        }
        return new(root, templates.ToImmutable());
    }
}
