// SPDX-License-Identifier: MIT

namespace vm2.Abstractions.Benchmarks;

#if SHORT_RUN
[ShortRunJob]
#else
[SimpleJob(RuntimeMoniker.HostProcess)]
#endif
public class EchoBenchmarks
{
    private string _value = "payload";

    [Benchmark]
    public string Echo_Value() => AbstractionsApi.Echo(_value, "fallback");

    [Benchmark]
    public string Echo_Fallback() => AbstractionsApi.Echo(null, "fallback");
}
