#!/usr/bin/env bash

# There is currently no support for machine readable output from `dotnet list package --vulnerable`.
# See: https://github.com/NuGet/Home/issues/7752
# See: https://github.com/dotnet/sdk/issues/16852
#
# We were using Retire.NET but that has been deprecated as of 5.0.0 and does not run on .NET 6.0.
# See: https://github.com/RetireNet/dotnet-retire/issues/75

# Must be run first for clean environments.
dotnet restore

# Get the output of the package analysis.
$output = dotnet list package --vulnerable --include-transitive
Write-Output $output

# Check if the output contains "following vulnerable".
if ($output -match "following vulnerable") {
    exit 1
}