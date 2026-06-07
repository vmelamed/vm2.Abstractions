// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

namespace vm2.Abstractions;

/// <summary>
/// Accessor for the current tenant in a multi-tenant application. The current tenant is established once at the edge of the
/// application (typically by authentication middleware) and read by downstream layers to authorize and scope data access.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. MUST be a non-nullable type that implements <see cref="IEquatable{T}"/>.
/// </typeparam>
/// <remarks>
/// Typically the value <c>default(TTenantId)</c> (e.g., <see cref="Guid.Empty"/>, <c>0</c>, <c>null</c>, etc.) is reserved as
/// the sentinel for the unset state, as is in the default implementation. However, if <c>default(TTenantId)</c> is a valid
/// tenant identifier in your application, override <see cref="IsSet"/> to use a different sentinel value or detection mechanism
/// for the unset state.
/// <para>
/// The current tenant <b>is set once per context</b>, where <b>the context is implementation-defined</b> (e.g., by using
/// <see cref="AsyncLocal{T}"><c>AsyncLocal&lt;TTenantId&gt;</c></see> or
/// <see cref="ThreadLocal{T}"><c>ThreadLocal&lt;TTenantId&gt;</c></see>): implementations MUST throw
/// <see cref="InvalidOperationException"/> on attempts to change a tenant that has already been set in the current context,
/// while accepting re-sets to the same value as idempotent no-ops. By implementing <see cref="ITenanted{TTenantId}"/>, the
/// accessor can be passed directly to <see cref="ITenanted.SameTenantAs(ITenanted)">SameTenantAs</see> to enforce tenant
/// isolation at service or repository boundaries. This makes the accessor a structural authorization trust boundary that
/// downstream code cannot pivot to a different tenant.
/// </para>
/// </remarks>
public interface ICurrentTenant<TTenantId> : ITenanted<TTenantId> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// A predicate indicating whether a tenant has been set in the current asynchronous context.
    /// </summary>
    /// <remarks>
    /// The default implementation of <see cref="ICurrentTenant{TTenantId}.IsSet">IsSet</see> calls directly
    /// <see cref="ITenanted{TTenantId}.TenantId">TenantId</see> and compares it to <c>default(TTenantId)</c>, but if you choose
    /// another sentinel value or detection mechanism, you MUST override <see cref="IsSet"/>.
    /// </remarks>
    bool IsSet => !EqualityComparer<TTenantId>.Default.Equals(TenantId, default);

    /// <summary>
    /// Sets the current tenant identifier for the current context.
    /// </summary>
    /// <param name="tenantId">
    /// The tenant identifier to set for the current context. Should not equal the value of the unset sentinel (e.g.,
    /// <c>default(TTenantId)</c> for the default implementation).
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="tenantId"/> equals the value of the unset sentinel.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current tenant has already been set in this context to a different value.
    /// </exception>
    void SetCurrentTenant(TTenantId tenantId);
}
