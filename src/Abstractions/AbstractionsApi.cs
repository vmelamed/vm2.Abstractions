// SPDX-License-Identifier: MIT

namespace vm2.Abstractions;

/// <summary>
/// A minimal sample API for vm2.Abstractions.
/// </summary>
public static class AbstractionsApi
{
    /// <summary>
    /// Returns the input string or a default fallback when null.
    /// </summary>
    /// <param name="value">Input value.</param>
    /// <param name="fallback">Optional fallback when <paramref name="value"/> is null.</param>
    /// <returns>The original value when provided; otherwise the fallback.</returns>
    public static string Echo(string? value, string fallback = "default")
    {
        return value ?? fallback;
    }
}
