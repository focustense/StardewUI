using System.Text;
using StardewUI.Framework.Binding;
using StardewUI.Framework.Descriptors;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;
using StardewUI.Widgets;

namespace StardewUI.Framework.Api;

/// <summary>
/// A view based on a <see cref="Document"/>.
/// </summary>
/// <remarks>
/// This view type mainly exists as glue for the API, to be used in a <see cref="ViewMenu{T}"/>.
/// </remarks>
internal class DocumentView : DecoratorView, IDescriptorDependent, IDisposable
{
    private static readonly BackoffRule backoffRule = BackoffRule.Default;

    private readonly IViewNodeFactory viewNodeFactory;
    private readonly IValueSource<Document> documentSource;

    /// <summary>
    /// The data context (model) to provide to the root node.
    /// </summary>
    public BindingContext? Context
    {
        get => context;
        set
        {
            if (value != context)
            {
                context = value;
                if (rootNode is not null)
                {
                    rootNode.Context = value;
                }
            }
        }
    }

    private BackoffState? backoffState;
    private BindingContext? context;
    private IViewNode? rootNode;

    /// <summary>
    /// Initializes a new <see cref="DocumentView"/> instance.
    /// </summary>
    /// <param name="viewNodeFactory">Factory for creating and binding <see cref="IViewNode"/>s.</param>
    /// <param name="documentSource">Source providing the StarML document describing the view.</param>
    public DocumentView(IViewNodeFactory viewNodeFactory, IValueSource<Document> documentSource)
    {
        this.viewNodeFactory = viewNodeFactory;
        this.documentSource = documentSource;
        CodeReloadHandler.RegisterDependent(this);
    }

    public override void Dispose()
    {
        base.Dispose();
        rootNode?.Dispose();
        rootNode = null;
        if (documentSource is IDisposable sourceDisposable)
        {
            sourceDisposable.Dispose();
        }
        GC.SuppressFinalize(this);
    }

    public override void OnUpdate(TimeSpan elapsed)
    {
        using var _ = Trace.Begin(this, nameof(OnUpdate));
        if (backoffState is not null)
        {
            backoffState.Elapsed += elapsed;
        }
        if (documentSource.Update())
        {
            rootNode?.Dispose();
            rootNode = null;
        }
        if (rootNode is null)
        {
            CreateViewNode();
            if (rootNode is null)
            {
                return;
            }
        }
        if (rootNode.Update(elapsed))
        {
            View = rootNode.Views.FirstOrDefault();
        }
        base.OnUpdate(elapsed);
    }

    // InitialUpdate is used by the DocumentView to ensure that the view hierarchy is created. Otherwise, the real view
    // tree may not exist until the end of the frame, which is too late for certain menu tasks.
    internal void InitialUpdate()
    {
        documentSource.Update();
        CreateViewNode();
        rootNode?.Update(TimeSpan.Zero);
        View = rootNode?.Views.FirstOrDefault();
    }

    private void CreateViewNode()
    {
        using var _ = Trace.Begin(this, nameof(CreateViewNode));
        if (documentSource.Value is null)
        {
            return;
        }
        if (backoffState is not null && backoffState.Elapsed < backoffState.Duration)
        {
            return;
        }
        try
        {
            rootNode = viewNodeFactory.CreateNode(documentSource.Value);
            rootNode.Context = Context;
            backoffState = null;
        }
        catch (Exception ex)
        {
            if (backoffState is not null)
            {
                backoffState.Duration = backoffRule.GetNextDuration(backoffState.Duration);
                backoffState.Elapsed = TimeSpan.Zero;
            }
            else
            {
                backoffState = new(backoffRule.InitialDuration);
            }
            var messageBuilder = new StringBuilder().AppendLine("Failed to create root node.").AppendLine();
            if (ex is BindingException b && b.Node is SNode node)
            {
                messageBuilder.AppendLine("The error occurred at:");
                messageBuilder.Append("  ");
                IElement element = node.Element;
                bool hasChildNodes = node.ChildNodes.Count > 0;
                element.Print(messageBuilder, !hasChildNodes);
                if (hasChildNodes)
                {
                    messageBuilder.Append("...");
                    element.PrintClosingTag(messageBuilder);
                }
                messageBuilder.AppendLine();
            }
            messageBuilder.AppendLine().Append(ex);
            Logger.Log(messageBuilder.ToString(), LogLevel.Error);
        }
    }

    void IDescriptorDependent.InvalidateDescriptors()
    {
        rootNode?.Dispose();
        rootNode = null;
        Context = Context is not null ? BindingContext.Create(Context.Data, Context.Parent) : null;
        OnUpdate(TimeSpan.FromTicks(1));
    }
}
