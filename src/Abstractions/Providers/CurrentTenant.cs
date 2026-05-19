namespace vm2.Abstractions.Providers;

/// <summary>
/// Implements the current tenant accessor for a multi-tenant application using <see cref="AsyncLocal{T}"/> storage so that
/// the tenant identifier flows with the logical asynchronous call.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. Must be non-nullable and implement <see cref="IEquatable{T}"/>.
/// </typeparam>
/// <remarks>
/// The storage is <see langword="static"/> so that the accessor's behavior does not depend on its DI lifetime: whether
/// registered as a singleton, scoped, or transient service, every resolved instance reads and writes the same async-flowing
/// slot, eliminating a class of subtle tenant-bleed bugs caused by lifetime misconfiguration. Generic statics in .NET are
/// allocated per closed generic type, so <c>CurrentTenant&lt;Guid&gt;</c> and <c>CurrentTenant&lt;TTenantId&gt;</c> for any
/// other type parameter have entirely separate storage.
/// <para>
/// The value <c>default(TTenantId)</c> is reserved as the sentinel for the unset state. Attempts to set the tenant to
/// <c>default(TTenantId)</c> throw <see cref="ArgumentException"/>.
/// </para>
/// <para>
/// The current tenant is <b>set-once per asynchronous context</b>. After the tenant has been set, attempts to change it
/// throw <see cref="InvalidOperationException"/>; re-setting it to the same value is an idempotent no-op. This makes the
/// accessor a structural authorization trust boundary: the authentication middleware at the edge of the application writes
/// it once, and no downstream code can pivot to a different tenant.
/// </para>
/// </remarks>
public class CurrentTenant<TTenantId> : ICurrentTenant<TTenantId> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Process-wide, asynchronous-context-local storage for the current tenant identifier. Static so that all instances of
    /// <see cref="CurrentTenant{TTenantId}"/> share the same async-flowing slot regardless of DI lifetime.
    /// </summary>
    static readonly AsyncLocal<TTenantId> _tenantId = new();

    /// <summary>
    /// Gets a value indicating whether a tenant has been set in the current asynchronous context.
    /// </summary>
    public bool IsSet
        => !EqualityComparer<TTenantId>.Default.Equals(_tenantId.Value, default);

    /// <summary>
    /// Gets the identifier of the current tenant for the current asynchronous context/UoW.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the current tenant has not been set.</exception>
    public TTenantId TenantId
    {
        get => IsSet
                ? _tenantId.Value!
                : throw new InvalidOperationException("The current tenant is not set.");
        private set => _tenantId.Value = value;
    }

    /// <summary>
    /// Sets the current tenant identifier for the current asynchronous context/UoW. Set-once per context: re-setting to
    /// the same value is an idempotent no-op; attempting to change it throws <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <param name="tenantId">
    /// The identifier of the tenant to set as the current tenant. Must not equal <c>default(TTenantId)</c>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="tenantId"/> equals <c>default(TTenantId)</c>, which is reserved as the unset sentinel.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current tenant has already been set in this asynchronous context to a different value.
    /// </exception>
    public void SetCurrentTenant(TTenantId tenantId)
    {
        var comparer = EqualityComparer<TTenantId>.Default;

        if (comparer.Equals(tenantId, default))
            throw new ArgumentException(
                $"The tenant identifier cannot be the default value of {typeof(TTenantId).Name}. " +
                "That value is reserved as the unset sentinel.",
                nameof(tenantId));

        var current = _tenantId.Value;
        if (!comparer.Equals(current, default))
        {
            if (comparer.Equals(current, tenantId))
                return;

            throw new InvalidOperationException(
                "The current tenant is already set in this asynchronous context and cannot be changed.");
        }

        TenantId = tenantId;
    }
}
