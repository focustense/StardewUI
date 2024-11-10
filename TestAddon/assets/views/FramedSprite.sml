<lane vertical-content-alignment="middle">
	<image layout="48px 48px" sprite={@Mods/StardewUI/Sprites/SmallLeftArrow} tooltip="Previous Item" focusable="true" click=|PreviousItem()| />
	<frame background={@Mods/StardewUI/Sprites/ControlBorder} margin="16,0" padding="32">
		<image layout="64px" sprite={SelectedItemId} />
	</frame>
	<image layout="48px 48px" sprite={@Mods/StardewUI/Sprites/SmallRightArrow} tooltip="Next Item" focusable="true" click=|NextItem()| />
</lane>