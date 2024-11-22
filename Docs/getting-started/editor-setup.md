# Editor Setup

StardewUI doesn't require you to use any special editor, but [StarML](../framework/starml.md), like JSON or HTML, is much easier to write correctly when using an editor with syntax highlighting.

Some editors may behave acceptably if told to treat `.sml` as either HTML or XML, but the results vary widely across editors.

## Recommended Editors

These are the editors with full support for StarML, provided as extensions.

- [Visual Studio](https://visualstudio.microsoft.com/), using the [StardewUI Extension](https://github.com/focustense/StardewUI/releases/download/v{{{release.version}}}/StardewUI.VisualStudio.vsix). Best for C# developers on Windows.
- [Visual Studio Code](https://code.visualstudio.com/), using the [vscode-starml](https://github.com/focustense/StardewUI/releases/download/v{{{release.version}}}/vscode-starml-{{{release.version}}}.vsix) extension.

When using one of these editors with the corresponding extension installed, `.sml` (or `.starml`) files will be recognized as StarML and have correct highlighting for all types of syntax elements (tags, literal attributes, [structural attributes](../framework/starml.md#structural-attributes), all the different [attribute flavors](../framework/starml.md#attribute-flavors)).

## Alternative Editors

If you cannot use any of the recommended editors, then the following have been tested to have **partial/incomplete** highlighting:

- [Notepad++](https://notepad-plus-plus.org/) using the [StarML User Defined Language](https://raw.githubusercontent.com/focustense/StardewUI/refs/heads/dev/Tools/NotepadPlusPlus/StarML_by_focustense.xml) (see also: [Import a UDL](https://npp-user-manual.org/docs/user-defined-language-system/#import-a-udl))

These options produce sane results that highlight the basic structure of the document correctly, but may struggle with certain aspects; for example, [binding modifiers](../framework/starml.md#binding-modifiers) may be interpreted as tags or otherwise break the attribute highlighting.