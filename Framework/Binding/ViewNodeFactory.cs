using StardewUI.Framework.Content;
using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Default in-game view engine.
/// </summary>
/// <param name="viewFactory">Factory for creating views, based on their tag names.</param>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the types required
/// by the target view or structural property.</param>
/// <param name="viewBinder">Binding service used to create <see cref="IViewBinding"/> instances that detect changes to
/// data or assets and propagate them to the bound <see cref="IView"/>.</param>
/// <param name="assetCache">Cache for obtaining document assets. Used for included views.</param>
/// <param name="monitor">Monitor for logging events.</param>
public class ViewNodeFactory(
    IViewFactory viewFactory,
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IViewBinder viewBinder,
    IAssetCache assetCache,
    IMonitor? monitor
) : IViewNodeFactory
{
    /// <inheritdoc />
    public IViewNode CreateNode(SNode node)
    {
        return CreateNode(node, null);
    }

    record SwitchContext(IViewNode Node, IAttribute Attribute);

    private IViewNode CreateNode(SNode node, SwitchContext? switchContext)
    {
        var innerNode = CreateNodeWithoutBackoff(node, switchContext);
        return new BackoffNodeDecorator(innerNode, BackoffRule.Default, monitor);
    }

    private IViewNode CreateNodeWithoutBackoff(SNode node, SwitchContext? switchContext)
    {
        var structuralAttributes = StructuralAttributes.Get(node.Attributes);
        if (structuralAttributes.Repeat is IAttribute repeatAttr)
        {
            return new RepeaterNode(valueSourceFactory, CreateNonRepeatingNode, repeatAttr);
        }
        else
        {
            return CreateNonRepeatingNode();
        }

        IViewNode CreateDefaultViewNode()
        {
            return new ViewNode(
                valueSourceFactory,
                viewFactory,
                viewBinder,
                node.Element,
                contextAttribute: structuralAttributes.Context
            );
        }

        IViewNode CreateIncludedViewNode()
        {
            var assetNameAttribute = node.Attributes.SingleOrDefault(attr =>
                attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase)
            );
            if (assetNameAttribute is null)
            {
                throw new BindingException("<include> node is missing a 'name' attribute.");
            }
            return new IncludedViewNode(
                valueSourceFactory,
                valueConverterFactory,
                assetCache,
                doc => CreateNode(doc.Root, switchContext),
                assetNameAttribute,
                structuralAttributes.Context
            );
        }

        IViewNode CreateNonRepeatingNode()
        {
            var viewNode = node.Tag.Equals("include", StringComparison.OrdinalIgnoreCase)
                ? CreateIncludedViewNode()
                : CreateDefaultViewNode();
            IViewNode result = viewNode;
            if (structuralAttributes.Case is IAttribute caseAttr)
            {
                if (switchContext is null)
                {
                    throw new BindingException(
                        $"Cannot bind *case attribute (value: '{caseAttr.Value}') without a prior *switch."
                    );
                }
                var condition = new BinaryCondition(
                    valueSourceFactory,
                    valueConverterFactory,
                    switchContext.Attribute,
                    caseAttr
                )
                {
                    LeftContextSelector = () => switchContext.Node.Context,
                };
                result = new ConditionalNode(viewNode, condition);
            }
            if (structuralAttributes.If is not null)
            {
                var condition = new UnaryCondition(valueSourceFactory, valueConverterFactory, structuralAttributes.If);
                result = new ConditionalNode(result, condition);
            }
            var nextSwitchContext = structuralAttributes.Switch is IAttribute switchAttr
                ? new SwitchContext(viewNode, switchAttr)
                : switchContext;
            if (viewNode is ViewNode defaultViewNode)
            {
                defaultViewNode.ChildNodes = node.ChildNodes.Select(n => CreateNode(n, nextSwitchContext)).ToList();
            }
            return result;
        }
    }

    class StructuralAttributes
    {
        public IAttribute? Case { get; set; }
        public IAttribute? Context { get; set; }
        public IAttribute? If { get; set; }
        public IAttribute? Repeat { get; set; }
        public IAttribute? Switch { get; set; }

        public static StructuralAttributes Get(IReadOnlyList<IAttribute> attributes)
        {
            var result = new StructuralAttributes();
            foreach (var attribute in attributes)
            {
                switch (attribute.Name)
                {
                    case "case":
                        result.Case = attribute;
                        break;
                    case "context":
                        result.Context = attribute;
                        break;
                    case "if":
                        result.If = attribute;
                        break;
                    case "repeat":
                        result.Repeat = attribute;
                        break;
                    case "switch":
                        result.Switch = attribute;
                        break;
                }
            }
            return result;
        }
    }
}
