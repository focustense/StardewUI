<lane orientation="vertical" horizontal-content-alignment="middle">
    <lane vertical-content-alignment="middle">
        <image layout="48px 48px"
               horizontal-alignment="middle"
               vertical-alignment="middle"
               sprite={@Mods/StardewUI/Sprites/SmallLeftArrow}
               focusable="true"
               click=|PreviousMonster()| />
        <banner layout="350px content"
                margin="16, 0"
                background={@Mods/StardewUI/Sprites/BannerBackground}
                background-border-thickness="48, 0"
                padding="12"
                text={SelectedMonsterName} />
        <image layout="48px 48px"
               horizontal-alignment="middle"
               vertical-alignment="middle"
               sprite={@Mods/StardewUI/Sprites/SmallRightArrow}
               focusable="true"
               click=|NextMonster()| />
    </lane>
    <lane>
        <lane layout="150px content"
              margin="0, 16, 0, 0"
              orientation="vertical"
              horizontal-content-alignment="end"
              z-index="2">
            <frame *repeat={AllTabs}
                   layout="120px 64px"
                   margin={Margin}
                   padding="16, 0"
                   horizontal-content-alignment="middle"
                   vertical-content-alignment="middle"
                   background={@Mods/focustense.StardewUITest/Sprites/MenuTiles:TabButton}
                   focusable="true"
                   click=|^SelectTab(Value)|>
                <label text={Value} />
            </frame>
        </lane>
        <frame *switch={SelectedTab}
               layout="400px 300px"
               margin="0, 16, 0, 0"
               padding="32, 24"
               background={@Mods/StardewUI/Sprites/ControlBorder}>
            <lane *case="General"
                  *context={SelectedMonster}
                  layout="stretch content"
                  orientation="vertical"
                  horizontal-content-alignment="middle">
                <panel *context={CurrentAnimation}
                       layout="stretch 128px"
                       margin="0, 0, 0, 12"
                       horizontal-content-alignment="middle"
                       vertical-content-alignment="middle">
                    <image layout={Layout} sprite={Sprite} />
                </panel>
                <label margin="0, 8" color="#136" text={Name} />
                <lane *if={HasDangerousVariant}
                      margin="0, 16"
                      vertical-content-alignment="middle">
                    <checkbox layout="content 32px" label-text="Dangerous" is-checked={<>IsDangerousSelected} />
                </lane>
            </lane>
            <lane *case="Combat"
                  *context={SelectedMonster}
                  orientation="vertical">
                <lane margin="0, 0, 0, 6" vertical-content-alignment="middle">
                    <image layout="20px content" sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:HealthIcon} />
                    <label layout="140px content" margin="8, 0" text="Health" />
                    <label color="#136" text={Health} />
                </lane>
                <lane margin="0, 6" vertical-content-alignment="middle">
                    <image layout="20px content" sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:AttackIcon} />
                    <label layout="140px content" margin="8, 0" text="Attack" />
                    <label color="#136" text={Attack} />
                </lane>
                <lane margin="0, 6" vertical-content-alignment="middle">
                    <image layout="20px content" sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:DefenseIcon} />
                    <label layout="140px content" margin="8, 0" text="Defense" />
                    <label color="#136" text={Defense} />
                </lane>
                <lane margin="0, 6" vertical-content-alignment="middle">
                    <image layout="20px content" sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:SpeedIcon} />
                    <label layout="140px content" margin="8, 0" text="Speed" />
                    <label color="#136" text={Speed} />
                </lane>
                <lane margin="0, 6" vertical-content-alignment="middle">
                    <image layout="20px content" sprite={@Mods/focustense.StardewUITest/Sprites/Cursors:LuckIcon} />
                    <label layout="140px content" margin="8, 0" text="Hit Chance" />
                    <label color="#136" text={Accuracy} />
                </lane>
            </lane>
            <grid *case="Loot"
                  *context={SelectedMonster}
                  layout="stretch"
                  item-layout="count: 5"
                  item-spacing="16, 16"
                  horizontal-item-alignment="middle">
              <lane *repeat={Drops} orientation="vertical" horizontal-content-alignment="middle">
                  <image layout="64px" margin="0, 0, 0, 4" sprite={Item} />
                  <label color={ChanceColor} scale="0.66" text={FormattedChance} />
              </lane>
            </grid>
        </frame>
        <spacer layout="50px content" />
    </lane>
</lane>