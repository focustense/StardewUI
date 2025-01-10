namespace StardewUI.Framework.Behaviors;

/// <summary>
/// Base class for a behavior extension, which enables self-contained, stateful behaviors to be "attached" to an
/// arbitrary view without having to extend the view itself.
/// </summary>
/// <remarks>
/// Behaviors receive the <see cref="View"/> which is decorated by the behavior, and some arbitrary <see cref="Data"/>
/// obtained from the attribute value or binding. They then become part of the UI's update loop, via their
/// <see cref="Update"/> method running every tick.
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
            if (value is not null)
            {
                OnNewData(previousData);
            }
        }
    }

    /// <summary>
    /// The currently-attached view.
    /// </summary>
    protected TView View => (TView)target!.View;

    /// <summary>
    /// State overrides for the <see cref="View"/>.
    /// </summary>
    protected IViewState ViewState => target!.ViewState;

    Type IViewBehavior.DataType => typeof(TData);

    private TData? data;
    private bool isDisposed;
    private BehaviorTarget? target;

    /// <inheritdoc />
    public bool CanUpdate()
    {
        return target is not null && data is not null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        isDisposed = true;
        if (target is not null)
        {
            OnDetached(target.View);
        }
        OnDispose();
        target = null;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Initialize(BehaviorTarget target)
    {
        if (target.View is not TView)
        {
            throw new ArgumentException(
                $"Target view is the wrong type (expected an instance of {typeof(TView).FullName}, got "
                    + $"{target.View.GetType().FullName}.",
                nameof(target)
            );
        }
        var previousView = this.target?.View;
        var previousViewState = this.target?.ViewState;
        this.target = target;
        if (target.View == previousView && target.ViewState == previousViewState)
        {
            return;
        }
        if (previousView is not null)
        {
            OnDetached(previousView);
        }
        OnAttached();
    }

    /// <inheritdoc />
    public virtual void PreUpdate(TimeSpan elapsed) { }

    /// <inheritdoc />
    public virtual void Update(TimeSpan elapsed) { }

    /// <summary>
    /// Runs after the behavior is attached to a target.
    /// </summary>
    /// <remarks>
    /// Setup code should go in this method to ensure that the values of <see cref="View"/> and <see cref="ViewState"/>
    /// are actually assigned. If code runs in the behavior's constructor, these are not guaranteed to be populated.
    /// </remarks>
    protected virtual void OnAttached() { }

    /// <summary>
    /// Runs when the behavior is detached from a target.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Behaviors may receive new views as part of a "rebind", if the old view is destroyed and recreated, for example
    /// as the result of a conditional binding changing states.
    /// </para>
    /// <para>
    /// <c>OnDetached</c> is always immediately followed by <see cref="OnAttached"/>. A behavior cannot remain in a
    /// detached state; however, overriding this method gives behaviors the opportunity to clean up state from the old
    /// view (e.g. remove event handlers) before the new one is attached.
    /// </para>
    /// <para>
    /// Also runs when the behavior is disposed, so detach logic does not need to be duplicated in
    /// <see cref="OnDispose"/>.
    /// </para>
    /// </remarks>
    /// <param name="view">The view that was previously attached.</param>
    protected virtual void OnDetached(IView view) { }

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
                    + $"{typeof(TData).FullName} or a subtype). The behavior will be disabled until it receives data "
                    + "with a supported type.",
                LogLevel.Warn
            );
            Data = default!;
            return;
        }
        Data = data is not null ? (TData)data : default!;
    }
}
