using System.Text;
using StardewUI.Framework.Descriptors;

namespace StardewUI.Framework.Binding;

/// <summary>
/// A transparent binding node that propagates <see cref="IViewNode.Update(TimeSpan)"/> ticks to an eligible context.
/// </summary>
/// <param name="node">The owned node.</param>
/// <param name="tracker">Shared instance tracking all context updates per frame.</param>
public class ContextUpdatingNodeDecorator(IViewNode node, ContextUpdateTracker tracker) : IViewNode
{
    private const string METHOD_NAME = "Update";

    /// <inheritdoc />
    public IReadOnlyList<IViewNode.Child> Children => node.Children;

    /// <inheritdoc />
    public BindingContext? Context
    {
        get => node.Context;
        set
        {
            if (value == node.Context)
            {
                return;
            }
            node.Context = value;
            AttachToContext(value);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IView> Views => node.Views;

    private Action<TimeSpan>? dispatchUpdate;

    /// <inheritdoc />
    public void Dispose()
    {
        dispatchUpdate = null;
        node.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Print(StringBuilder sb, bool includeChildren)
    {
        node.Print(sb, includeChildren);
    }

    /// <inheritdoc />
    public void Reset()
    {
        node.Reset();
    }

    /// <inheritdoc />
    public bool Update(TimeSpan elapsed)
    {
        if (Context?.Data is not null && !tracker.WasAlreadyUpdated(Context.Data))
        {
            tracker.TrackUpdate(Context.Data);
            dispatchUpdate?.Invoke(elapsed);
        }
        return node.Update(elapsed);
    }

    private void AttachToContext(BindingContext? context)
    {
        dispatchUpdate = null;
        if (context?.Data is null || !context.Descriptor.TryGetMethod(METHOD_NAME, out var updateMethod))
        {
            return;
        }
        if (updateMethod.ReturnType != typeof(void))
        {
            Logger.LogOnce(
                $"Ignoring {METHOD_NAME} method on type {context.Data.GetType().FullName} for update dispatches "
                    + "because it does not have a void return type.",
                LogLevel.Warn
            );
            return;
        }
        if (
            updateMethod.ArgumentTypes.Length > 1
            || (updateMethod.ArgumentTypes.Length == 1 && updateMethod.ArgumentTypes[0] != typeof(TimeSpan))
        )
        {
            Logger.LogOnce(
                $"Ignoring {METHOD_NAME} method on type {context.Data.GetType().FullName} for update dispatches "
                    + "because it has the wrong argument types (must take either no parameters or a single "
                    + $"{nameof(TimeSpan)} argument).",
                LogLevel.Warn
            );
            return;
        }
        if (updateMethod is not IMethodDescriptor<object> voidMethod)
        {
            Logger.LogOnce(
                $"Ignoring {METHOD_NAME} method on type {context.Data.GetType().FullName} for update dispatches "
                    + $"because its descriptor is not an instance of {typeof(IMethodDescriptor<object>).Name}.",
                LogLevel.Warn
            );
            return;
        }
        dispatchUpdate =
            voidMethod.ArgumentTypes.Length == 1
                ? elapsed => voidMethod.Invoke(context.Data, [elapsed])
                : _ => voidMethod.Invoke(context.Data, []);
    }
}
