<lane orientation="vertical">
	<lane margin="32, 0, 0, -16" z-index="1">
		<tab *repeat={Tabs}
             layout="64px"
             active={<>Active}
             tooltip={Name}
             activate=|^OnTabActivated(Name)|>
			<image layout="32px" sprite={Sprite} vertical-alignment="middle" />
		</tab>
	</lane>
	<frame layout="600px 500px"
	       background={@Mods/StardewUI/Sprites/MenuBackground}
           border={@Mods/StardewUI/Sprites/MenuBorder}
           border-thickness="36, 36, 40, 36"
		   padding="8">
		<label text="content" />
	</frame>
</lane>