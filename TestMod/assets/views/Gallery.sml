<frame layout="50%[720..1280] 80%[600..]"
       background={@Mods/StardewUI/Sprites/MenuBackground}
       border={@Mods/StardewUI/Sprites/MenuBorder}
       border-thickness="36, 36, 40, 36"
       opacity="0"
       transform="scale: 0; rotate: -90"
       transform-origin="0.5, 0.5"
       +show:opacity="1"
       +show:transform="scale: 1; rotate: 0"
       +transition:opacity="600ms"
       +transition:transform="400ms 50ms EaseInOutElastic">
    <scrollable peeking="128">
        <lane layout="stretch content"
              margin="16, 16, 16, 8"
              orientation="vertical">
            <label layout="stretch content"
                   margin="0, 8, 0, 16"
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

<template name="example">
    <frame layout="stretch content"
           margin="0, 8, 0, 7"
           padding="16"
           background={@Mods/StardewUI/Sprites/White}
           background-tint="#0000"
           border={@Mods/StardewUI/Sprites/MenuSlotTransparent}
           border-thickness="4"
           focusable="true"
           +hover:background-tint="#14c8"
           +transition:background-tint="350ms EaseOutSine"
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