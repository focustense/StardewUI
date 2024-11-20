<panel layout="400px" horizontal-content-alignment="middle" vertical-content-alignment="middle">
    <image layout="stretch"
           fit="stretch"
           sprite={@Mods/StardewUI/Sprites/ControlBorder}
           tooltip="Background"
           click=|Log("Background clicked")| />
    <image layout="200px 140px"
           sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:QueenOfSauce1}
           z-index="1"
           tooltip="Image"
           click=|Log("Image clicked")| />
    <image layout="300px"
           fit="stretch"
           sprite={@Mods/StardewUI/Sprites/ButtonDark}
           tooltip="Button"
           click=|Log("Button clicked")| />
</panel>