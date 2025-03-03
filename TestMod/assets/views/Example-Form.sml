<lane orientation="vertical"
      horizontal-content-alignment="middle"
      opacity={Opacity}>
    <!-- Example Comment -->
    <banner background={@Mods/StardewUI/Sprites/BannerBackground}
            background-border-thickness="48,0"
            padding="12"
            text={#focustense.StardewUI:SettingsMenu.Title} />
    <frame layout="700px content"
           margin="0,8,0,0"
           padding="32,24"
           background={@Mods/StardewUI/Sprites/ControlBorderUncolored}
           background-tint={BackgroundTint}>
        <lane layout="stretch content" orientation="vertical">
            <form-heading color={TextColor} text={#Example.Form.Profile.Heading} />
            <form-row color={TextColor} title={#Example.Form.Name.Title}>
                <textinput layout="250px 48px"
                           margin="-6, 0, 0, 0"
                           max-length="20"
                           text={Name} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.PetType.Title}>
                <dropdown option-min-width="200"
                          options={:PetTypes}
                          selected-option={<>SelectedPetType} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.TomatoType.Title}>
                <dropdown option-min-width="200"
                          options={:TomatoTypes}
                          selected-option={<>SelectedTomatoType} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.Difficulty.Title}>
                <frame background={@Mods/StardewUI/Sprites/MenuSlotInsetUncolored} background-tint={BackgroundTint} padding="4">
                    <segments highlight={@Mods/StardewUI/Sprites/White}
                              highlight-tint="#38c"
                              highlight-transition="150ms EaseOutCubic"
                              separator={@Mods/StardewUI/Sprites/White}
                              separator-tint="#333"
                              separator-width="2">
                        <label margin="12, 8"
                               color={TextColor}
                               text={#Example.Form.Difficulty.Easy.Title}
                               tooltip={#Example.Form.Difficulty.Easy.Description} />
                        <label margin="12, 8"
                               color={TextColor}
                               text={#Example.Form.Difficulty.Normal.Title}
                               tooltip={#Example.Form.Difficulty.Normal.Description} />
                        <label margin="12, 8"
                               color={TextColor}
                               text={#Example.Form.Difficulty.Hard.Title}
                               tooltip={#Example.Form.Difficulty.Hard.Description}/>
                    </segments>
                </frame>
            </form-row>
            
            <form-heading text={#Example.Form.UI.Heading} />
            <form-row color={TextColor} title={#Example.Form.ColorTheme.Title}>
                <theme-picker *repeat={:Themes} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.Opacity.Title}>
                <slider track-width="240"
                        min="0.05"
                        max="1"
                        interval="0.05"
                        value={<>Opacity}
                        value-format={:FormatOpacity} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.TextColor.Title}>
                <color-picker layout="300px content" color={<>TextColor} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.Keybind.Title}>
                <keybind-editor button-height="64"
                                sprite-map={@Mods/StardewUI/SpriteMaps/Buttons:default-default-0.5}
                                editable-type="MultipleKeybinds"
                                add-button-text={#Example.Form.Keybind.Add}
                                empty-text={#Example.Form.Keybind.Empty}
                                focusable="true"
                                keybind-list={<>Keybinds} />
            </form-row>
            <form-row color={TextColor} title={#Example.Form.AllowClose.Title}>
                <checkbox is-checked={<>AllowClose} />
            </form-row>
        </lane>
    </frame>
    <lane layout="stretch content"
          margin="16, 8, 0, 0"
          horizontal-content-alignment="end"
          vertical-content-alignment="middle">
        <button text={#Example.Form.Button.Cancel}
                hover-background={@Mods/StardewUI/Sprites/ButtonLight}
                opacity="0.5"
                +state:enabled={AllowClose}
                +state:enabled:opacity="1.0"
                left-click=|Close("false")| />
        <button margin="16, 0, 0, 0"
                text={#Example.Form.Button.OK}
                hover-background={@Mods/StardewUI/Sprites/ButtonLight}
                left-click=|Close("false")| />
    </lane>
</lane>

<template name="form-heading">
    <label margin="0,0,0,8"
           font="dialogue"
           color={&color}
           text={&text}
           shadow-alpha="0.6"
           shadow-layers="VerticalAndDiagonal"
           shadow-offset="-3, 3" />
</template>

<template name="form-row">
    <lane layout="stretch content" margin="16,4" vertical-content-alignment="middle">
        <label layout="280px content"
               margin="0,8"
               color={&color}
               text={&title}
               shadow-alpha="0.8"
               shadow-color="#4448"
               shadow-offset="-2, 2" />
        <outlet />
    </lane>
</template>

<template name="theme-picker">
    <panel layout="48px" margin="8, 0">
        <image layout="stretch"
               fit="stretch"
               margin="8"
               sprite={@Mods/StardewUI/Sprites/White}
               tint={:Color}
               focusable="true"
               left-click=|^SetTheme(this)| />
        <image *if={Selected}
               layout="stretch"
               fit="stretch"
               sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:SelectionRect} />
    </panel>
</template>