<lane orientation="vertical" horizontal-content-alignment="middle" opacity={Opacity}>
    <banner background={@Mods/StardewUI/Sprites/BannerBackground} background-border-thickness="48,0" padding="12" text={#focustense.StardewUI:SettingsMenu.Title} />
    <frame layout="500px content" background={@Mods/StardewUI/Sprites/ControlBorder} margin="0,8,0,0" padding="32,24">
        <lane layout="stretch content" orientation="vertical">
            <form-heading text="Speed" />
            <form-row title={#ExampleForm.TurboBoost.Title}>
                <checkbox is-checked={<>EnableTurboBoost} />
            </form-row>
            <form-row title={#ExampleForm.SpeedMultiplier.Title}>
                <slider min="10" max="100" interval="1" value={<>SpeedMultiplier} />
            </form-row>
            <form-row title="Menu Opacity">
                <slider min="0" max="1" interval="0.1" value={<>Opacity} />
            </form-row>
            <form-row title="Allow Closing">
                <checkbox is-checked={<>AllowClose} />
            </form-row>
        </lane>
    </frame>
</lane>

<template name="form-heading">
    <label font="dialogue" margin="0,0,0,8" text={&text} shadow-alpha="0.6" shadow-layers="VerticalAndDiagonal" shadow-offset="-3, 3" />
</template>

<template name="form-row">
    <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
        <label layout="280px content" margin="0,8" text={&title} />
        <outlet />
    </lane>
</template>