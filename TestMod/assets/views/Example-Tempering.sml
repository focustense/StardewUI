<frame layout="60%[..800] content"
	   background={@Mods/StardewUI/Sprites/MenuBackgroundUncolored}
       background-tint="#9ce"
       border={@Mods/StardewUI/Sprites/MenuBorderUncolored}
       border-tint="#69b"
       border-thickness="36, 36, 40, 36"
       padding="8"
       button-press=|HandleButtonPress($Button)|>
    <frame *float="above"
           layout="stretch content"
           horizontal-content-alignment="middle">
        <banner background={@Mods/StardewUI/Sprites/BannerBackground}
                background-border-thickness="48, 0"
                padding="12"
                text="Tool Tempering" />
    </frame>
    <frame *repeat={Statuses}
           *float="after; 0, 18"
           background={@Mods/StardewUI/Sprites/ControlBorderUncolored}
           background-tint="#9df"
           padding="24"
           opacity={Opacity}
           +transition:opacity={Transition}>
        <lane orientation="vertical">
            <label bold="true" text={Name} />
            <image layout="stretch 4px"
                   margin="0, 12, 0, 0"
                   fit="stretch"
                   sprite={@Mods/StardewUI/Sprites/ThinHorizontalDividerUncolored}
                   tint="#ace" />
            <label *if={IsEmpty} margin="0, 8, 0, 0" text="No active effects" />
            <lane *repeat={:CombinedMaterials}
                  margin="0, 8, 0, 0"
                  vertical-content-alignment="middle">
                <image layout="32px" sprite={:Item} />
                <label margin="8, 0, 0, 0" bold={WasQuantityChanged} color={TextColor} text={DisplayText} />
            </lane>
        </lane>
    </frame>
    <lane layout="stretch content" orientation="vertical">
        <lane pointer-leave=|ShowToolStatus(SelectedTool)|>
            <tool-selector *repeat={Tools} tool={this} sprite={ItemData} selected={IsSelected} />
        </lane>
        <image layout="stretch content"
               margin="-44, -12, -48, -12"
               fit="stretch"
               sprite={@Mods/StardewUI/Sprites/MenuHorizontalDividerUncolored}
               tint="#69b"
               pointer-events-enabled="false" />
        <panel *context={SelectedTool} layout="stretch content" clip-size="stretch">
            <lane layout="stretch content"
                  padding="0, 0, 0, 8"
                  orientation="vertical"
                  transform="translateX: -1000"
                  opacity="0"
                  +show:transform=""
                  +transition:transform="250ms EaseOutCubic"
                  +show:opacity="1"
                  +transition:opacity="500ms">
                <lane vertical-content-alignment="middle">
                    <label font="dialogue"
                           shadow-alpha="0.8"
                           shadow-color="#999"
                           shadow-layers="VerticalAndDiagonal"
                           shadow-offset="-3, 3"
                           text={:DisplayName} />
                    <spacer layout="stretch 0px" />
                    <panel tooltip={UsageTooltip}>
                        <alloy-gauge filled="false" />
                        <alloy-gauge filled="true" clip-size={FinalMaterialProgressClipLayout} tint="#0c0" />
                        <alloy-gauge filled="true" clip-size={CurrentMaterialProgressClipLayout} />
                    </panel>
                </lane>
                <frame layout="stretch content"
                       margin="0, 16"
                       padding="8, 12"
                       border={@Mods/StardewUI/Sprites/MenuSlotInsetUncolored}
                       border-tint="#eee">
                    <grid layout="stretch content"
                          item-layout="length: 80+"
                          horizontal-item-alignment="middle">
                        <material-breakdown sprite={BaseMaterialItem}
                                            echoes=""
                                            amount={UsageLabel}
                                            color="#620"
                                            bold="true"
                                            tooltip={BaseMaterialTooltip} />
                        <material-breakdown *repeat={FinalComposition}
                                            sprite={:Item}
                                            echoes={Echoes}
                                            amount={Quantity}
                                            bold={WasQuantityChanged}
                                            color={TextColor}
                                            tooltip={EffectTooltip} />
                    </grid>
                </frame>
                <grid layout="stretch content"
                      item-layout="length: 84+"
                      item-spacing="1, 2"
                      horizontal-item-alignment="middle">
                    <material-input *repeat={^AvailableMaterials}
                                    sprite={:Item}
                                    quantity={Quantity}
                                    tint={BackgroundColor}
                                    opacity={SlotOpacity}
                                    tooltip={EffectTooltip} />
                </grid>
                <image layout="stretch 4px"
                       margin="0, 8"
                       fit="stretch"
                       sprite={@Mods/StardewUI/Sprites/ThinHorizontalDividerUncolored}
                       tint="#ace" />
                <lane layout="stretch content"
                      vertical-content-alignment="middle">
                    <label *if={CanUpgrade} margin="0, 0, 8, 0" text="Will consume:" />
                    <material-cost *repeat={Cost} sprite={Item} amount={Quantity} tooltip={Item} />
                    <spacer layout="stretch 0px" />
                    <button margin="16, 0, 4, 0"
                            default-background={@Mods/focustense.StardewUITest/Sprites/Cursors2:ButtonUncolored}
                            default-background-tint="#3c3"
                            hover-background-tint="#3c3"
                            +hover:default-background-tint="#3f6"
                            +hover:hover-background-tint="#3f6"
                            +transition:default-background-tint="250ms"
                            +transition:hover-background-tint="250ms"
                            opacity={UpgradeButtonOpacity}
                            +transition:opacity="150ms">
                        <lane horizontal-content-alignment="middle">
                            <image layout="32px" margin="0, 0, 8, 0" sprite={@Item/(W)55} />
                            <label text="Temper" />
                        </lane>
                    </button>
                </lane>
            </lane>
        </panel>
    </lane>
</frame>

<template name="tool-selector">
    <panel layout="112px"
           margin="0, 0, 16, 0"
           horizontal-content-alignment="middle"
           vertical-content-alignment="middle"
           focusable="true">
        <image *if={&selected}
               layout="stretch"
               sprite={@Mods/focustense.StardewUITest/Sprites/shapes:OctagonFilled}
               tint="#e72" />
        <image *if={&selected}
               layout="stretch"
               sprite={@Mods/focustense.StardewUITest/Sprites/shapes:Octagon}
               tint="#039" />
        <image layout="64px"
               sprite={&sprite}
               shadow-alpha="0.2"
               shadow-offset="-4, 4"
               transform-origin="0.5, 0.5"
               +hover:transform="scale: 1.25"
               +transition:transform="250ms EaseOutCubic"
               left-click=|^SelectTool(&tool)|
               pointer-enter=|^ShowToolStatus(&tool)|
               pointer-leave=|^ShowToolStatus()| />
    </panel>
</template>

<template name="alloy-gauge">
    <lane clip-size={&clip-size} +transition:clip-size="250ms EaseOutQuart">
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
        <alloy-slot filled={&filled} tint={&tint} />
    </lane>
</template>

<template name="alloy-slot">
    <panel layout="32px" margin="2, 0">
        <image layout="stretch"
               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:LargeStar}
               tint="#0006" />
        <image *if={&filled}
               layout="stretch"
               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:LargeStar}
               tint={&tint}
               transform="scale: 0"
               transform-origin="0.5, 0.5"
               +show:transform="scale: 1"
               +transition:transform="500ms EaseOutElastic" />
    </panel>
</template>

<template name="material-breakdown">
    <lane orientation="vertical" horizontal-content-alignment="middle" tooltip={&tooltip}>
        <panel layout="64px">
            <image layout="stretch"
                   sprite={&sprite}
                   shadow-alpha="0.2"
                   shadow-offset="-4, 4"
                   focusable="true" />
            <image *repeat={&echoes}
                   layout="stretch"
                   sprite={&sprite}
                   pointer-events-enabled="false"
                   opacity="0.8"
                   +show:opacity="0"
                   +transition:opacity="400ms"
                   transform-origin="0.5, 0.5"
                   +show:transform="scale: 1.75"
                   +transition:transform="400ms EaseOutCubic" />
        </panel>
        <label margin="0, 8, 0, 0"
               text={&amount}
               color={&color}
               bold={&bold}
               shadow-alpha="0.8"
               shadow-color="#999"
               shadow-layers="VerticalAndDiagonal"
               shadow-offset="-2, 2" />
    </lane>
</template>

<template name="material-input">
    <frame layout="stretch content"
           background={@Mods/StardewUI/Sprites/MenuBackgroundUncolored}
           background-tint={&tint}
           border={@Mods/StardewUI/Sprites/MenuSlotTransparentUncolored}
           border-thickness="4"
           border-tint="#9ab"
           opacity={&opacity}
           +transition:opacity="200ms"
           padding="4, 0, 4, 8"
           horizontal-content-alignment="middle"
           focusable="true"
           tooltip={&tooltip}
           click=|^AddMaterial(this)|
           +hover:background-tint="#f93"
           +transition:background-tint="200ms">
        <panel horizontal-content-alignment="end"
               vertical-content-alignment="end">
            <image layout="64px" sprite={&sprite} shadow-alpha="0.2" shadow-offset="-3, 3" />
            <digits margin="-4, -4" number={&quantity} />
        </panel>
    </frame>
</template>

<template name="material-cost">
    <panel margin="4, 0" horizontal-content-alignment="end" vertical-content-alignment="end">
        <image margin="4" layout="32px" sprite={&sprite} shadow-alpha="0.2" shadow-offset="-2, 2" />
        <digits number={&amount} />
    </panel>
</template>