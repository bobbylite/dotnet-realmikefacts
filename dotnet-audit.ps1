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
if ($output -match "Project ``bobbylite.realmikefacts.web`` has the following vulnerable packages") {
    exit 0 # change to 1 to cause failure in pipeline.
}
else {
    Write-Output "Project ``bobbylite.realmikefacts.web`` has no vulnerable packages"
}