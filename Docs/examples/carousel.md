# Example: Train Shop (Carousel) [:material-file-code:](https://github.com/focustense/StardewUI/blob/dev/TestAddon/CarouselMenuViewModel.cs){ title="C# Source (Model)" } [:material-file-code-outline:](https://github.com/focustense/StardewUI/blob/dev/TestAddon/Carousel.cs){ title="C# Source (View)" } [:material-file-star-outline:](https://github.com/focustense/StardewUI/blob/dev/TestAddon/assets/views/CarouselDemo.sml){ title="StarML Source" }

_Author: [:material-github:focustense](https://github.com/focustense)_
{ .example-header }

<div class="grid cards dense" markdown>

- :material-table-cog: Bindings
- :material-mouse-left-click: Events
- :octicons-stack-16: Repeaters
- :material-tag-hidden: Conditionals
- :material-grid: Grid
- :material-star-cog: View Addon
- :material-cog-transfer: Converter Addon
- :material-motion: Animation

</div>

A very showy, very hypothetical example of a "shop" where you can buy different items from different train cars.

This example isn't meant to be very practical, but shows off how to use add-ons, AKA [framework extensions](../framework/extensions.md), and includes some unusual features you're unlikely to find anywhere else, such as tweening motion, sub-layout and pagination using controller buttons.

!!! note

    Because this example is more code-heavy than most, the code tabs below may be abridged or organized differently from the original source. Click the source links next to the page title to see the full source code.

=== "Demo"

    <video controls>
      <source src="../../videos/example-carousel.mp4" type="video/mp4">
    </video>

=== "CarouselMenuViewModel.cs"

    ```cs
    internal partial class CarouselMenuViewModel
    {
        public int HeaderIndex => SelectedPageIndex + 1;
        public List<CarouselMenuPage> Pages { get; set; } = [];
    
        [Notify] private int selectedPageIndex;
    
        public bool HandleButtonPress(SButton button)
        {
            return button switch
            {
                SButton.LeftShoulder => PreviousPage(),
                SButton.RightShoulder => NextPage(),
                _ => false
            };
        }
    
        public bool NextPage()
        {
            if (SelectedPageIndex < (Pages.Count - 1))
            {
                SelectedPageIndex++;
                return true;
            }
            return false;
        }
    
        public bool PreviousPage()
        {
            if (SelectedPageIndex > 0)
            {
                SelectedPageIndex--;
                return true;
            }
            return false;
        }
    }
    
    internal enum TrainCarType { Passenger, Freight }
    
    internal enum TrainCovering { None, Minerals, Boxes }
    
    internal class CarouselMenuPage
    {
        public string Title { get; } = title;
        public TrainCarType CarType { get; } = carType;
        public TrainCovering Covering { get; } = covering;
        public IReadOnlyList<PurchasableItem> Items { get; set; } = items ?? [];
    }
    
    internal class PurchasableItem(ParsedItemData data, int price)
    {
        public ParsedItemData Data { get; } = data;
        public string DisplayName => Data.DisplayName;
        public int Price { get; } = price;
    }
    ```

=== "Carousel.cs"

    ```cs
    internal partial class Carousel : View
    {
        public IList<IView> Children { ... }
        public float Gap { ... }
        public LayoutParameters SelectionLayout { ... }
    
        [Notify] private KeySpline easing = KeySpline.Linear;
        [Notify] private int selectedIndex;
        [Notify] private float transitionDuration = 500; // milliseconds
    
        private readonly List<ViewChild> childPositions = [];
    
        private float drawingOffset;
        private float selectedOffset;
        private float transitionProgress;
        private float transitionStartOffset;
    
        public override void OnUpdate(TimeSpan elapsed)
        {
            if (drawingOffset == selectedOffset)
            {
                return;
            }
            transitionProgress += (float)elapsed.TotalMilliseconds;
            if (transitionProgress >= TransitionDuration)
            {
                drawingOffset = selectedOffset;
                transitionProgress = 0;
                return;
            }
            float progressRatio = transitionProgress / TransitionDuration;
            float offsetRatio = Easing.Get(progressRatio);
            drawingOffset = transitionStartOffset +
                (selectedOffset - transitionStartOffset) * offsetRatio;
        }
    
        protected override FocusSearchResult? FindFocusableDescendant(
            Vector2 contentPosition,
            Direction direction)
        {
            if (Children.Count == 0)
            {
                return null;
            }
            var selectedChild =
                (SelectedIndex >= 0 && SelectedIndex < Children.Count)
                    ? childPositions[SelectedIndex]
                    : childPositions[0];
            var offset = new Vector2(selectedOffset, 0);
            return selectedChild
                .FocusSearch(contentPosition + offset, direction)?
                .Offset(-offset);
        }
    
        protected override IEnumerable<ViewChild> GetLocalChildren()
        {
            var offset = new Vector2(-selectedOffset, 0);
            return childPositions.Select(c => c.Offset(offset));
        }
    
        protected override void OnDrawContent(ISpriteBatch b)
        {
            using var _clip = b.Clip(new(0, 0, (int)OuterSize.X, (int)OuterSize.Y));
            var clipBounds = new Bounds(Vector2.Zero, OuterSize);
            foreach (var child in childPositions)
            {
                if (!child.GetActualBounds()
                          .Offset(new(-drawingOffset, 0))
                          .IntersectsWith(clipBounds))
                {
                    continue;
                }
                using var _ = b.SaveTransform();
                b.Translate(child.Position.X - drawingOffset, child.Position.Y);
                child.View.Draw(b);
            }
        }
    
        protected override void OnMeasure(Vector2 availableSize)
        {
            ContentSize = Layout.GetLimits(availableSize);
            var selectionLimits = SelectionLayout.GetLimits(availableSize);
            childPositions.Clear();
            float x = (ContentSize.X - selectionLimits.X) / 2;
            foreach (var childView in children)
            {
                childView.Measure(selectionLimits);
                float top = (ContentSize.Y - childView.OuterSize.Y) / 2;
                childPositions.Add(new(childView, new(x, top)));
                x += childView.OuterSize.X + Gap;
            }
            selectedOffset = GetStartOffset(SelectedIndex);
        }
    
        private void BeginTransition()
        {
            selectedOffset = GetStartOffset(SelectedIndex);
            transitionStartOffset = drawingOffset;
            transitionProgress = 0;
        }
    
        private float GetStartOffset(int index)
        {
            if (childPositions.Count == 0
                || index < 0
                || index >= childPositions.Count)
            {
                return 0;
            }
            var child = childPositions[index];
            var localOffset = (ContentSize.X - child.View.OuterSize.X) / 2;
            return child.Position.X - localOffset;
        }
    
        // This method has a special name recognized by
        // PropertyChanged.SourceGenerator, and will run whenever the
        // SelectedIndex property changes to a new value.
        private void OnSelectedIndexChanged()
        {
            BeginTransition();
        }
    }
    ```

=== "CarouselDemo.sml"

    ```html
    <lane vertical-content-alignment="middle">
        <image layout="64px"
               sprite={@Mods/StardewUI/Sprites/LargeLeftArrow}
               tooltip="Previous Car"
               focusable="true"
               left-click=|PreviousPage()| />
        <frame layout="800px 800px"
               background={@Mods/StardewUI/Sprites/MenuBackground}
               border={@Mods/StardewUI/Sprites/MenuBorder}
               border-thickness="36, 36, 40, 36"
               button-press=|HandleButtonPress($Button)|>
            <lane orientation="vertical">
                <carousel layout="stretch 350px"
                          selection-layout="480px 240px"
                          easing="InOutCubic"
                          selected-index={HeaderIndex}>
                    <image layout="stretch"
                           sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarFreightCovered} />
                    <panel *repeat={Pages} *switch={CarType} layout="stretch">
                        <image *case="Passenger"
                               layout="stretch"
                               sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarPassenger} />
                        <image *case="Freight"
                               layout="stretch"
                               sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCarFreightUncovered} />
                        <panel *switch={Covering}
                               layout="stretch"
                               margin="26, 15, 22, 18">
                            <image *case="Minerals"
                                   layout="stretch"
                                   sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCargoMinerals} />
                            <image *case="Boxes"
                                   layout="stretch"
                                   sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainCargoBoxes} />
                        </panel>
                    </panel>
                    <image layout="stretch"
                           sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:TrainEngine} />
                </carousel>
                <carousel layout="stretch"
                          selection-layout="600px stretch"
                          easing="OutCubic"
                          gap="50"
                          selected-index={SelectedPageIndex}>
                    <frame *repeat={Pages}
                           layout="stretch"
                           margin="0, 0, 0, 32"
                           padding="16"
                           background={@Mods/StardewUI/Sprites/ControlBorder}>
                        <lane orientation="vertical" padding="12">
                            <banner layout="stretch content" text={Title} />
                            <grid layout="stretch"
                                  margin="0, 16, 0, 0"
                                  item-layout="count: 3"
                                  item-spacing="16, 16"
                                  horizontal-item-alignment="middle">
                                <frame *repeat={Items}
                                       layout="stretch content"
                                       padding="12"
                                       background={@Mods/focustense.StardewUITestAddon/Sprites/MenuTiles:ItemBorder}
                                       tooltip={DisplayName}
                                       focusable="true">
                                    <lane layout="stretch content"
                                          orientation="vertical"
                                          horizontal-content-alignment="middle">
                                        <image layout="64px" sprite={Data} />
                                        <lane margin="0, 8" vertical-content-alignment="middle">
                                            <image layout="22px"
                                                   sprite={@Mods/focustense.StardewUITestAddon/Sprites/Cursors:Coin} />
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
        <image layout="64px"
               sprite={@Mods/StardewUI/Sprites/LargeRightArrow}
               tooltip="Next Car"
               focusable="true"
               left-click=|NextPage()| />
    </lane>
    ```