namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Base class for a behavior extension, which enables self-contained, stateful behaviors to be "attached" to an
/// arbitrary view without having to extend the view itself.
/// </summary>
/// <remarks>
/// <para>
/// Behaviors receive the <see cref="View"/> which is decorated by the behavior, and some arbitrary <see cref="Data"/>
/// obtained from the attribute value or binding. They then become part of the UI's update loop, via their
/// <see cref="Update"/> method running every tick.
/// </para>
/// <para>
/// Several lifecycle hooks are defined to handle either the <see cref="View"/> or <see cref="Data"/> changing.
/// Implementers should always use these hooks to handle changes and <b>never</b> save their own copy of either, in
/// whole or in part, to avoid memory or resource leaks.
/// </para>
/// <para>
/// This type is not thread-safe. The <see cref="Data"/> and <see cref="View"/> properties are guaranteed to be non-null
/// when <see cref="Update"/> runs, and are typed accordingly, because the framework will skip executing behaviors that
/// have null data or no attached view; however, if a behavior is accessed from a different context, especially on a
/// different thread, these properties may transiently contain null values.
/// </para>
/// </remarks>
/// <typeparam name="TView">Base type for all views that support this behavior.</typeparam>
/// <typeparam name="TData">Type of data provided to this behavior as an argument/binding.</typeparam>
public abstract class ViewBehavior<TView, TData> : IViewBehavior
    where TView : IView
{
    /// <summary>
    /// The assigned or bound data.
    /// </summary>
    public TData Data
    {
        get => data!;
        set
        {
            if (EqualityComparer<TData>.Default.Equals(value, data))
            {
                return;
            }
            var previousData = data;
            data = value;
            OnNewData(previousData);
        }
    }

    /// <summary>
    /// The currently-attached view.
    /// </summary>
    public TView View
    {
        get => view!;
        set
        {
            if (EqualityComparer<TView>.Default.Equals(value, view))
            {
                return;
            }
            if (view is not null)
            {
                OnDetach(view);
            }
            view = value;
            if (value is not null)
            {
                OnAttach(value);
            }
        }
    }

    Type IViewBehavior.DataType => typeof(TData);
    Type IViewBehavior.ViewType => typeof(TView);

    private TData? data;
    private TView? view;
    private bool isDisposed;

    /// <inheritdoc />
    public bool CanUpdate()
    {
        return view is not null && data is not null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        OnDispose();
        // Don't allow a dangling behavior to keep the view alive.
        if (view is not null)
        {
            OnDetach(view);
        }
        view = default;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public abstract void Update(TimeSpan elapsed);

    /// <summary>
    /// Runs when the <see cref="View"/> is first attached to this behavior.
    /// </summary>
    /// <remarks>
    /// Common reasons to override this method are to add an event handler to one of the view's events, or to save some
    /// part of the view's initial state so that it can be reset in <see cref="OnDetach(TView)"/>.
    /// </remarks>
    /// <param name="view">The view being attached.</param>
    protected virtual void OnAttach(TView view) { }

    /// <summary>
    /// Runs when a previously-attached view is detached, either due to the <see cref="View"/> changing or the behavior
    /// being disposed.
    /// </summary>
    /// <remarks>
    /// Common reasons to override this method are to remove an event handler from one of the view's events, or to
    /// restore some initial state of the view that was saved in <see cref="OnAttach(TView)"/>.
    /// </remarks>
    /// <param name="view">The view being detached.</param>
    protected virtual void OnDetach(TView view) { }

    /// <summary>
    /// Runs when the behavior is being disposed.
    /// </summary>
    /// <remarks>
    /// The default implementation does nothing. Overriding this allows subclasses to perform their own cleanup, if
    /// required by the specific feature.
    /// </remarks>
    protected virtual void OnDispose() { }

    /// <summary>
    /// Runs when the <see cref="Data"/> of this behavior is changed.
    /// </summary>
    /// <remarks>
    /// At the time this method runs, <see cref="Data"/> has already been assigned to the new value. After the method
    /// completes, the <paramref name="previousData"/> will no longer be accessible to this behavior.
    /// </remarks>
    /// <param name="previousData"></param>
    protected virtual void OnNewData(TData? previousData) { }

    void IViewBehavior.SetData(object? data)
    {
        if (data is not null && !data.GetType().IsAssignableTo(typeof(TData)))
        {
            Logger.Log(
                $"Behavior type {GetType().FullName} cannot accept a data type of {data.GetType().FullName} (requires "
                    + $"{typeof(TData).FullName} or a subtype). The behavior will be disabled until it receives data with "
                    + "a supported type.",
                LogLevel.Warn
            );
            Data = default!;
            return;
        }
        // These null-forgiving assignments are valid because the actual implementations of the properties handle null
        // values implicitly, and because the framework is required to honor the result CanUpdate.
        Data = (TData)data!;
    }

    void IViewBehavior.SetView(IView? view)
    {
        if (view is not null && !view.GetType().IsAssignableTo(typeof(TView)))
        {
            Logger.Log(
                $"Behavior type {GetType().FullName} cannot accept a view type of {view.GetType().FullName} (requires "
                    + $"{typeof(TView).FullName} or a subtype). The behavior will be disabled until it receives a view "
                    + "with a supported type.",
                LogLevel.Warn
            );
            View = default!;
            return;
        }
        View = (TView)view!;
    }
}
