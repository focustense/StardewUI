<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={@Mods/StardewUI/Sprites/BannerBackground} background-border-thickness="48,0" padding="12" text={HeaderText} />
    <frame layout="880px 640px" background={@Mods/StardewUI/Sprites/ControlBorder} margin="0,16,0,0" padding="32,24">
        <scrollable peeking="128">
			<grid layout="stretch content" item-layout="length: 64" item-spacing="16,16" horizontal-item-alignment="middle">
				<image layout="stretch content" *repeat={Items} sprite={this} tooltip={DisplayName} focusable="true" />
			</grid>
		</scrollable>
    </frame>
</lane>