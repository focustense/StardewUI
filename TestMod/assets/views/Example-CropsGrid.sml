<lane layout="content 700px" orientation="horizontal" horizontal-content-alignment="start">
    <frame layout="400px stretch" background={@Mods/StardewUI/Sprites/ControlBorder} horizontal-content-alignment="middle" padding="32, 20">
        <lane orientation="vertical" *context={SelectedCrop} horizontal-content-alignment="middle">
            <banner text={Name} />
            <lane orientation="horizontal" >
                <image sprite={Seed} layout="64px 64px" margin="4"/>
                <image sprite={Produce} layout="64px 64px" margin="4"/>
            </lane>
            <grid layout="400px content" item-layout="count: 7" item-spacing="8,8" horizontal-item-alignment="middle">
                <frame *repeat={Harvest} layout="64px 64px" background={@Mods/StardewUI/Sprites/ButtonLight}>
                    <image *if={this} layout="48px 48px" sprite={^Produce} margin="8"/>
                </frame>
            </grid>
            <image sprite={Phase} layout="64px 128px" margin="0,16,0,0"/>
        </lane>
    </frame>
    <frame layout="600px stretch" background={@Mods/StardewUI/Sprites/ControlBorder} padding="20">
        <scrollable>
            <grid layout="content content" item-layout="count: 6" item-spacing="8, 8" horizontal-item-alignment="middle">
                <image *repeat={AllCrops} sprite={Seed} focusable="true" click=|^SelectCrop(this)| tooltip={Name} layout="64px 64px" />
            </grid>
        </scrollable>
    </frame>
</lane>
