// SPDX-License-Identifier: MIT

namespace vm2.Abstractions.Tests;

public class AbstractionsApiTests(ITestOutputHelper outputHelper) : TestBase(outputHelper)
{
    [Fact]
    public void Echo_returns_value_when_present()
    {
        var result = AbstractionsApi.Echo("hi", "fallback");
        result.Should().Be("hi");
    }

    [Fact]
    public void Echo_returns_fallback_when_null()
    {
        var result = AbstractionsApi.Echo(null, "fallback");
        result.Should().Be("fallback");
    }
}
