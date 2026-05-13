#!/usr/bin/env dotnet

// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2026 Val Melamed

#:property TargetFramework=net10.0
#:project ../src/Abstractions/Abstractions.csproj

using static System.Console;
using static System.Text.Encoding;

using vm2.Abstractions;

using static vm2.Abstractions.AbstractionsApi;

Console.WriteLine("Abstractions example");

Console.WriteLine(Echo("hello", "fallback"));
Console.WriteLine(Echo(null, "fallback"));
