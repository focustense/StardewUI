using StardewUI.Framework.Behaviors;
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
/// <param name="resolutionScopeFactory">Factory for creating <see cref="IResolutionScope"/> instances responsible for
/// resolving external symbols such as translation keys.</param>
/// <param name="behaviorFactory">Factory for creating behavior extensions.</param>
public class ViewNodeFactory(
    IViewFactory viewFactory,
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IViewBinder viewBinder,
    IAssetCache assetCache,
    IResolutionScopeFactory resolutionScopeFactory,
    IBehaviorFactory behaviorFactory
) : IViewNodeFactory
{
    /// <inheritdoc />
    public IViewNode CreateNode(Document document)
    {
        var scope = resolutionScopeFactory.CreateForDocument(document);
        var nodeTransformers = GetNodeTransformers(document);
        return CreateNode(document.Root, nodeTransformers, scope);
    }

    /// <inheritdoc />
    public IViewNode CreateNode(
        SNode node,
        IReadOnlyList<INodeTransformer> nodeTransformers,
        IResolutionScope resolutionScope
    )
    {
        return CreateNodeChild(node, nodeTransformers, null, resolutionScope).Node;
    }

    record SwitchContext(IViewNode Node, IAttribute Attribute, IResolutionScope ResolutionScope);

    private IViewNode.Child CreateNodeChild(
        SNode node,
        IReadOnlyList<INodeTransformer> nodeTransformers,
        SwitchContext? switchContext,
        IResolutionScope resolutionScope
    )
    {
        using var _ = Trace.Begin(this, nameof(CreateNode));
        var (innerNode, outletName) = CreateNodeChildWithoutBackoff(
            node,
            nodeTransformers,
            switchContext,
            resolutionScope
        );
        var backoffDecorator = new BackoffNodeDecorator(innerNode, BackoffRule.Default);
        var updatingDecorator = new ContextUpdatingNodeDecorator(backoffDecorator, ContextUpdateTracker.Instance);
        return new(updatingDecorator, outletName);
    }

    private IViewNode.Child CreateNodeChildWithoutBackoff(
        SNode node,
        IReadOnlyList<INodeTransformer> nodeTransformers,
        SwitchContext? switchContext,
        IResolutionScope resolutionScope
    )
    {
        var structuralAttributes = StructuralAttributes.Get(node.Attributes);
        if (structuralAttributes.Repeat is IAttribute repeatAttr)
        {
            return new(new RepeaterNode(valueSourceFactory, CreateNonRepeatingNodeChild, resolutionScope, repeatAttr));
        }
        else
        {
            return CreateNonRepeatingNodeChild();
        }

        IViewNode CreateDefaultViewNode()
        {
            var behaviorAttributes = node.Attributes.Where(attr => attr.Type == Grammar.AttributeType.Behavior);
            var behaviors = new ViewBehaviors(
                behaviorAttributes,
                behaviorFactory,
                valueSourceFactory,
                valueConverterFactory,
                resolutionScope
            );
            return new ViewNode(
                valueSourceFactory,
                valueConverterFactory,
                viewFactory,
                viewBinder,
                node.Element,
                resolutionScope,
                behaviors,
                contextAttribute: structuralAttributes.Context,
                floatAttribute: structuralAttributes.Float
            );
        }

        IViewNode CreateIncludedViewNode()
        {
            var assetNameAttribute = node.Attributes.SingleOrDefault(attr =>
                attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase)
            );
            if (assetNameAttribute is null)
            {
                throw new BindingException("<include> node is missing a 'name' attribute.", node);
            }
            return new IncludedViewNode(
                valueSourceFactory,
                valueConverterFactory,
                assetCache,
                resolutionScope,
                doc =>
                    CreateNodeChild(
                        doc.Root,
                        GetNodeTransformers(doc),
                        switchContext,
                        resolutionScopeFactory.CreateForDocument(doc)
                    ).Node,
                assetNameAttribute,
                structuralAttributes.Context
            );
        }

        IViewNode.Child CreateNonRepeatingNodeChild()
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
                        $"Cannot bind *case attribute (value: '{caseAttr.Value}') without a prior *switch.",
                        node
                    );
                }
                var condition = new BinaryCondition(
                    valueSourceFactory,
                    valueConverterFactory,
                    switchContext.ResolutionScope,
                    switchContext.Attribute,
                    resolutionScope,
                    caseAttr
                )
                {
                    LeftContextSelector = () => switchContext.Node.Context,
                }.NegateIf(caseAttr.IsNegated);
                result = new ConditionalNode(viewNode, condition);
            }
            if (structuralAttributes.If is not null)
            {
                var condition = new UnaryCondition(
                    valueSourceFactory,
                    valueConverterFactory,
                    resolutionScope,
                    structuralAttributes.If
                ).NegateIf(structuralAttributes.If.IsNegated);
                result = new ConditionalNode(result, condition);
            }
            var nextSwitchContext = structuralAttributes.Switch is IAttribute switchAttr
                ? new SwitchContext(viewNode, switchAttr, resolutionScope)
                : switchContext;
            if (viewNode is ViewNode defaultViewNode)
            {
                var childNodes = node.ChildNodes.AsEnumerable();
                foreach (var transformer in nodeTransformers)
                {
                    childNodes = childNodes.SelectMany(transformer.Transform);
                }
                defaultViewNode.Children = childNodes
                    .Select(n => CreateNodeChild(n, nodeTransformers, nextSwitchContext, resolutionScope))
                    .ToList();
            }
            return new(result, structuralAttributes.Outlet?.Value);
        }
    }

    private static IReadOnlyList<INodeTransformer> GetNodeTransformers(Document document)
    {
        // Transformers could be cached by document, but since TemplateNodeTransformer is just a wrapper around the
        // template that we already have access to here, it's unlikely to be worth the cost.
        return document.Templates.Select(template => new TemplateNodeTransformer(template)).ToArray();
    }

    class StructuralAttributes
    {
        public IAttribute? Case { get; set; }
        public IAttribute? Context { get; set; }
        public IAttribute? Float { get; set; }
        public IAttribute? If { get; set; }
        public IAttribute? Outlet { get; set; }
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
                        ThrowIfNegated(attribute);
                        result.Context = attribute;
                        break;
                    case "float":
                        ThrowIfNegated(attribute);
                        result.Float = attribute;
                        break;
                    case "if":
                        result.If = attribute;
                        break;
                    case "outlet":
                        ThrowIfNegated(attribute);
                        result.Outlet = attribute;
                        break;
                    case "repeat":
                        ThrowIfNegated(attribute);
                        result.Repeat = attribute;
                        break;
                    case "switch":
                        ThrowIfNegated(attribute);
                        result.Switch = attribute;
                        break;
                }
            }
            return result;
        }

        private static void ThrowIfNegated(IAttribute attribute)
        {
            if (attribute.IsNegated)
            {
                throw new BindingException(
                    $"Invalid negation for structural attribute '{attribute.Name}'. Only conditional attributes such "
                        + "as *if and *case may be negated."
                );
            }
        }
    }
}
