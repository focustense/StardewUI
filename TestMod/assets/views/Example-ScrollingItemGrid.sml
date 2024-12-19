<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={@Mods/StardewUI/Sprites/BannerBackground} background-border-thickness="48,0" padding="12" text={HeaderText} />
    <frame layout="920px 640px" background={@Mods/StardewUI/Sprites/ControlBorder} margin="0,16,0,0" padding="16">
        <scrollable peeking="128">
			<grid layout="stretch content"
                  padding="16,8"
                  item-layout="length: 64+"
                  item-spacing="16,16"
                  horizontal-item-alignment="middle">
				<image *repeat={Items}
                       layout="stretch 64px"
                       sprite={this}
                       tooltip={:this}
                       focusable="true"
                       transform-origin="0.5, 0.5"
                       +hover:transform="scale: 1.4"
                       +transition:transform="700ms EaseOutElastic" />
			</grid>
		</scrollable>
    </frame>
</lane>