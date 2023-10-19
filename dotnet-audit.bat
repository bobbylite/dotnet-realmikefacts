:: There is currently no support for machine readable output from `dotnet list package --vulnerable`.
:: See: https://github.com/NuGet/Home/issues/7752
:: See: https://github.com/dotnet/sdk/issues/16852
::
:: We were using Retire.NET but that has been deprecated as of 5.0.0 and does not run on .NET 6.0.
:: See: https://github.com/RetireNet/dotnet-retire/issues/75

@echo off
:: Must be run first for clean environments.
dotnet restore

:: Get the output of the package analysis
for /f "tokens=*" %%i in ('dotnet list package --vulnerable --include-transitive') do (
    set "output=%%i"
    echo !output!
)

:: Check if the output contains "following vulnerable"
echo !output! | find "following vulnerable" > nul

:: If found, exit with an error code
if %errorlevel% equ 0 (
    exit /b 1
)
