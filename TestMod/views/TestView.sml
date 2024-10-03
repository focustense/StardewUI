<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={{@Mods/StardewUI/Sprites/BannerBackground}} background-border-thickness="48,0" padding="12" text={{HeaderText}} />
    <frame background={{@Mods/StardewUI/Sprites/ControlBorder}} margin="0,16,0,0" padding="32,24">
        <lane orientation="vertical" horizontal-content-alignment="middle">
            <label font="dialogue" text="Hello, world!" margin="0,0,0,8" />
            <label text="This is an example paragraph." />
            <panel margin="0,8,0,0" horizontal-content-alignment="middle" vertical-content-alignment="middle">
                <image layout="stretch" fit="stretch" sprite={{@Mods/StardewUI/Sprites/MenuSlotInset}} />
                <image layout="64px 64px" margin="8" sprite={{ItemData}} />
            </panel>
        </lane>
    </frame>
</lane>