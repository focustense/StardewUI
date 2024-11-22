<panel layout="50%[720..1280] 80%[600..]">
	<image layout="stretch"
           fit="stretch"
           margin="36, 36, 40, 36"
           sprite={@Mods/StardewUI/Sprites/MenuBackground}
           scale={BackgroundScale} />
	<image layout="stretch"
           fit="stretch"
           sprite={@Mods/StardewUI/Sprites/MenuBorder}
           scale={BackgroundScale} />
	<frame margin="40" padding="8" opacity={ContentOpacity}>
        <scrollable peeking="128">
            <lane layout="stretch content" orientation="vertical">
                <label layout="stretch content"
                       margin="0, 0, 0, 16"
                       font="dialogue"
                       horizontal-alignment="middle"
                       text={#Gallery.Title}
                       scale="1.25"
                       shadow-alpha="1"
                       shadow-offset="-3, 3" />
                <label layout="stretch content" margin="0, 0, 0, 8" text={#Gallery.Welcome} />
                <example *repeat={Examples} />
            </lane>
        </scrollable>
	</frame>
</panel>

<template name="example">
    <frame layout="stretch content"
           margin="0, 8, 0, 7"
           padding="16"
           background={@Mods/StardewUI/Sprites/White}
           background-tint={BackgroundTint}
           border={@Mods/StardewUI/Sprites/MenuSlotTransparent}
           border-thickness="4"
           focusable="true"
           pointer-enter=|StartHover()|
           pointer-leave=|EndHover()|
           left-click=|Open()|>
        <lane layout="stretch content" vertical-content-alignment="middle">
            <image margin="0, 0, 32, 0" sprite={:Thumbnail} />
            <lane layout="stretch content" orientation="vertical">
                <banner margin="0, 0, 0, 8" text={:Title} />
                <label text={:Description} />
            </lane>
        </lane>
    </frame>
</template>