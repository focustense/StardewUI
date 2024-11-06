<frame layout="800px 800px"
       background={@Mods/StardewUI/Sprites/MenuBackground}
       border={@Mods/StardewUI/Sprites/MenuBorder}
       border-thickness="36, 36, 40, 36"
       button-press=|HandleButtonPress($Button)|>
    <lane orientation="vertical">
        <carousel layout="stretch 350px" selection-layout="480px 240px" easing="InOutCubic" selected-index={HeaderIndex}>
            <image layout="stretch" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarFreightCovered} />
            <panel layout="stretch" *repeat={Pages} *switch={CarType}>
                <image *case="Passenger" layout="stretch" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarPassenger} />
                <image *case="Freight" layout="stretch" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarFreightUncovered} />
                <panel layout="stretch" margin="26, 15, 22, 18" *switch={Covering}>
                    <image layout="stretch" *case="Minerals" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCargoMinerals} />
                    <image layout="stretch" *case="Boxes" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCargoBoxes} />
                </panel>
            </panel>
            <image layout="stretch" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainEngine} />
        </carousel>
        <carousel layout="stretch" selection-layout="600px stretch" easing="OutCubic" gap="50" selected-index={SelectedPageIndex}>
            <frame *repeat={Pages} layout="stretch" margin="0, 0, 0, 32" background={@Mods/StardewUI/Sprites/ControlBorder} padding="16">
                <lane orientation="vertical" padding="12">
                    <banner layout="stretch content" text={Title} />
                    <grid layout="stretch" margin="0, 16, 0, 0" item-layout="count: 3" item-spacing="16, 16" horizontal-item-alignment="middle">
                        <frame *repeat={Items} layout="stretch content" background={@Mods/focustense.StardewUITestAddon/Sprites/MenuTiles:ItemBorder} padding="12" tooltip={DisplayName} focusable="true">
                            <lane layout="stretch content" orientation="vertical" horizontal-content-alignment="middle">
                                <image layout="64px" sprite={Data} />
                                <lane margin="0, 8" vertical-content-alignment="middle">
                                    <image layout="22px" sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:Coin} />
                                    <label margin="8, 0, 0, 0" text={Price} />
                                </lane>
                            </lane>
                        </frame>
                    </grid>
                </lane>
            </frame>
        </carousel>
    </lane>
</frame>