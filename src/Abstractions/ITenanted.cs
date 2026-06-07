// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

namespace vm2.Abstractions;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant.
/// </summary>
public interface ITenanted
{
    /// <summary>
    /// Determines whether this object is bound to the same tenant as <paramref name="otherTenanted"/>.
    /// </summary>
    /// <param name="otherTenanted">The other tenanted object to compare tenants.</param>
    /// <returns>
    /// <see langword="true"/> if this and the <paramref name="otherTenanted"/> objects are bound to the same tenant;
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool SameTenantAs(ITenanted otherTenanted);
}

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant and expose the tenant identifier.
/// </summary>
/// The type of the tenant identifier. MUST be a non-nullable type that implements <see cref="IEquatable{T}"/>.
public interface ITenanted<TTenantId> : ITenanted where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Gets the tenant identifier of this object.
    /// </summary>
    /// <remarks>
    /// By convention, the value <c>default(TTenantId)</c> (e.g., <see cref="Guid.Empty"/>, <c>0</c>, <c>null</c>) is used as
    /// the sentinel for the unset state and should not be used as a real tenant identifier unless you choose another value for
    /// the unset sentinel.
    /// </remarks>
    TTenantId TenantId { get; }

    /// <inheritdoc/>
    bool ITenanted.SameTenantAs(ITenanted otherTenant)
        => otherTenant is ITenanted<TTenantId> t
           && !EqualityComparer<TTenantId>.Default.Equals(TenantId, default)
           && EqualityComparer<TTenantId>.Default.Equals(TenantId, t.TenantId);
}
