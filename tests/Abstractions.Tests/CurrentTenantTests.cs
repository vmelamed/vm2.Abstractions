// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

namespace vm2.Abstractions.Tests;

public class CurrentTenantTests(ITestOutputHelper outputHelper) : TestBase(outputHelper)
{
    [Fact]
    public void TenantId_before_set_returns_default()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();

        var id = sut.TenantId;

        id.Should().Be(default(Guid));
    }

    [Fact]
    public void IsSet_is_false_before_set()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();

        sut.IsSet.Should().BeFalse();
    }

    [Fact]
    public void IsSet_is_true_after_set()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();

        sut.SetCurrentTenant(Guid.NewGuid());

        sut.IsSet.Should().BeTrue();
    }

    [Fact]
    public void SetCurrentTenant_then_TenantId_returns_value()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var id = Guid.NewGuid();

        sut.SetCurrentTenant(id);

        sut.TenantId.Should().Be(id);
    }

    [Fact]
    public void SetCurrentTenant_called_twice_with_same_value_is_idempotent()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var id = Guid.NewGuid();

        sut.SetCurrentTenant(id);
        var act = () => sut.SetCurrentTenant(id);

        act.Should().NotThrow();
        sut.TenantId.Should().Be(id);
    }

    [Fact]
    public void SetCurrentTenant_called_twice_with_different_value_throws_InvalidOperationException()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        sut.SetCurrentTenant(first);

        var act = () => sut.SetCurrentTenant(second);

        act.Should().Throw<InvalidOperationException>();
        sut.TenantId.Should().Be(first);
    }

    [Fact]
    public void SetCurrentTenant_with_default_Guid_throws_ArgumentException()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();

        var act = () => sut.SetCurrentTenant(Guid.Empty);

        act.Should().Throw<ArgumentException>()
           .And.ParamName.Should().Be("tenantId");
    }

    [Fact]
    public void SetCurrentTenant_with_zero_int_throws_ArgumentException()
    {
        ICurrentTenant<int> sut = new CurrentTenant<int>();

        var act = () => sut.SetCurrentTenant(0);

        act.Should().Throw<ArgumentException>()
           .And.ParamName.Should().Be("tenantId");
    }

    [Fact]
    public void SetCurrentTenant_with_null_string_throws_ArgumentException()
    {
        ICurrentTenant<string> sut = new CurrentTenant<string>();

        var act = () => sut.SetCurrentTenant(null!);

        act.Should().Throw<ArgumentException>()
           .And.ParamName.Should().Be("tenantId");
    }

    [Fact]
    public async Task TenantId_flows_across_await()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var id = Guid.NewGuid();
        sut.SetCurrentTenant(id);

        await Task.Yield();
        await Task.Delay(1, TestContext.Current.CancellationToken);

        sut.TenantId.Should().Be(id);
    }

    [Fact]
    public async Task TenantId_isolated_across_parallel_tasks()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var startGate = new TaskCompletionSource();

        async Task<Guid> RunWith(Guid id)
        {
            sut.SetCurrentTenant(id);
            await startGate.Task;
            return sut.TenantId;
        }

        var taskA = Task.Run(() => RunWith(idA));
        var taskB = Task.Run(() => RunWith(idB));

        startGate.SetResult();
        var results = await Task.WhenAll(taskA, taskB);

        results[0].Should().Be(idA);
        results[1].Should().Be(idB);
    }

    [Fact]
    public async Task Child_attempting_to_change_tenant_throws_InvalidOperationException()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var parentId = Guid.NewGuid();
        var childId = Guid.NewGuid();
        sut.SetCurrentTenant(parentId);

        var childAct = await Record.ExceptionAsync(
            () => Task.Run(() => sut.SetCurrentTenant(childId), TestContext.Current.CancellationToken));

        childAct.Should().BeOfType<InvalidOperationException>();
        sut.TenantId.Should().Be(parentId);
    }

    [Fact]
    public void Static_storage_is_shared_across_instances_of_same_closed_type()
    {
        ICurrentTenant<Guid> first = new CurrentTenant<Guid>();
        ICurrentTenant<Guid> second = new CurrentTenant<Guid>();
        var id = Guid.NewGuid();

        first.SetCurrentTenant(id);

        second.TenantId.Should().Be(id);
        second.IsSet.Should().BeTrue();
    }

    [Fact]
    public void Static_storage_is_isolated_across_different_closed_types()
    {
        ICurrentTenant<Guid> guidSut = new CurrentTenant<Guid>();
        ICurrentTenant<string> stringSut = new CurrentTenant<string>();

        guidSut.SetCurrentTenant(Guid.NewGuid());

        stringSut.IsSet.Should().BeFalse();
    }

    [Fact]
    public void Works_with_Guid_TenantId()
    {
        ICurrentTenant<Guid> sut = new CurrentTenant<Guid>();
        var id = Guid.NewGuid();

        sut.SetCurrentTenant(id);

        sut.TenantId.Should().Be(id);
    }

    [Fact]
    public void Works_with_string_TenantId()
    {
        ICurrentTenant<string> sut = new CurrentTenant<string>();

        sut.SetCurrentTenant("tenant-42");

        sut.TenantId.Should().Be("tenant-42");
    }
}
