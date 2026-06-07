// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

namespace vm2.Abstractions.Providers;

/// <summary>
/// Implements the current tenant accessor for a multi-tenant application using <see cref="AsyncLocal{T}"/> storage so that
/// the tenant identifier flows with the logical asynchronous call.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. MUST be a non-nullable type that implements <see cref="IEquatable{T}"/>.
/// </typeparam>
/// <remarks>
/// The storage is <see langword="static"/> so that the accessor's behavior does not depend on its DI lifetime: whether
/// registered as a singleton, scoped, or transient service, every resolved instance reads and writes the same async-flowing
/// slot, eliminating a class of subtle tenant-bleed bugs caused by lifetime misconfiguration. Generic statics in .NET are
/// allocated per closed generic type, so <c>CurrentTenant&lt;Guid&gt;</c> and <c>CurrentTenant&lt;TTenantId&gt;</c> for any
/// other type parameter have entirely separate storage.
/// <para>
/// The value <c>default(TTenantId)</c> is reserved as the sentinel for the unset state, i.e. leverages the default interface
/// implementation. Attempts to set the tenant to <c>default(TTenantId)</c> throw <see cref="ArgumentException"/>.
/// </para>
/// <para>
/// The current tenant is <b>set-once per asynchronous context</b>. After the tenant has been set, attempting to change it
/// throws <see cref="InvalidOperationException"/>; re-setting it to the same value is an idempotent no-op. This makes the
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
    /// Gets the identifier of the current tenant for the current asynchronous context.
    /// </summary>
    /// <remarks>
    /// If the tenant has not been set in the current asynchronous context, this property returns <c>default(TTenantId)</c>,
    /// which is the reserved sentinel value for the unset state and MUST NOT be used as a real tenant identifier. If the type
    /// parameter is a reference type, the getter returns <see langword="null"/> when the tenant is not set.
    /// </remarks>
    public TTenantId TenantId
    {
        get => _tenantId.Value!;
        private set => _tenantId.Value = value;
    }

    /// <summary>
    /// Sets the current tenant identifier for the current asynchronous context. Set-once per context: re-setting a non-default
    /// value to the same value is an idempotent no-op; attempting to re-set the current, non-default, tenant identifier to a
    /// different value throws <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <param name="tenantId">
    /// The identifier of the tenant to set as the current tenant. MUST NOT equal <c>default(TTenantId)</c>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="tenantId"/> equals <c>default(TTenantId)</c>, which is reserved as the unset sentinel.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current tenant has already been set in this asynchronous context to a different value.
    /// </exception>
    /// <remarks>
    /// Callers MUST ensure SetCurrentTenant is not called concurrently on the same logical context.
    /// </remarks>
    public void SetCurrentTenant([NotNull] TTenantId tenantId)
    {
        var comparer = EqualityComparer<TTenantId>.Default;

        if (comparer.Equals(tenantId, default))
            throw new ArgumentException(
                $"The tenant identifier cannot be the default value of {typeof(TTenantId).Name}. That value is reserved as the unset sentinel.",
                nameof(tenantId));

        var current = _tenantId.Value;

        if (comparer.Equals(current, default))
            TenantId = tenantId;
        else
        if (!comparer.Equals(current, tenantId))
            throw new InvalidOperationException("The current tenant is already set in this asynchronous context and cannot be changed.");
    }
}
