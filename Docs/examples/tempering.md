# Example: Tempering [:material-file-code-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/Examples/Tempering/TemperingViewModel.cs){ title="C# Source" } [:material-file-star-outline:](https://github.com/focustense/StardewUI/blob/dev/TestMod/assets/views/Example-Tempering.sml){ title="StarML Source" }

_Author: [:material-github:focustense](https://github.com/focustense)_
{ .example-header }

<div class="grid cards dense" markdown>

- :material-pac-man: Behaviors
- :material-layers: Floats
- :material-mouse-left-click: Events
- :material-tab: Tab-Like
- :material-circle-opacity: Transparency
- :material-seed: Templates
- :material-grid: Grid
- :material-motion: Animation

</div>

UI for a hypothetical mod that uses Stardew's mostly-useless minerals as "tempering" materials to add enchantment-like effects to tools, similar to classic ARPGs like Titan Quest. The UI is completely functional, though the underlying mod is not real.

This was done as a proof of concept and demo for the new [behaviors](../framework/behaviors.md) and transform/transition system introduced in version 0.5. It demonstrates almost every feature available in StardewUI.

Note that for some animations, it uses older techniques that have been superseded by newer features such as [named states](../framework/starml.md#behavior-attributes); the older methods are still valid, though they tend to require more code to implement.

<video controls>
  <source src="../../videos/example-tempering.mp4" type="video/mp4">
</video>

Due to the scope and complexity of this example, C# and StarML snippets are not provided directly on this page. View the [full C# source](https://github.com/focustense/StardewUI/tree/dev/TestMod/Examples/Tempering) and [StarML source](https://github.com/focustense/StardewUI/blob/dev/TestMod/assets/views/Example-Tempering.sml) for details.