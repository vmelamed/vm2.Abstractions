// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

namespace vm2.Abstractions;

/// <summary>
/// Accessor for the current tenant in a multi-tenant application. The current tenant is established once at the edge of the
/// application (typically by authentication middleware) and read by downstream layers to authorize and scope data access.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. Must be a non-nullable type that implements <see cref="IEquatable{T}"/>.
/// </typeparam>
/// <remarks>
/// The value <c>default(TTenantId)</c> (e.g., <see cref="Guid.Empty"/>, <c>0</c>, <c>null</c>) is reserved as the sentinel
/// for the unset state and must not be used as a real tenant identifier.
/// <para>
/// The current tenant is <b>set-once per asynchronous context</b>: implementations must throw
/// <see cref="InvalidOperationException"/> on attempts to change a tenant that has already been set in the current context,
/// while accepting re-sets to the same value as idempotent no-ops. This makes the accessor a structural authorization trust
/// boundary that downstream code cannot pivot.
/// </para>
/// </remarks>
public interface ICurrentTenant<TTenantId> : ITenanted<TTenantId> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Gets a value indicating whether a tenant has been set in the current asynchronous context.
    /// </summary>
    bool IsSet { get; }

    /// <summary>
    /// Sets the current tenant identifier for the current asynchronous context.
    /// </summary>
    /// <param name="tenantId">
    /// The identifier of the tenant to set as the current context. Must not equal <c>default(TTenantId)</c>, which is
    /// reserved as the unset sentinel.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="tenantId"/> equals <c>default(TTenantId)</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current tenant has already been set in this asynchronous context to a different value.
    /// </exception>
    void SetCurrentTenant(TTenantId tenantId);
}
