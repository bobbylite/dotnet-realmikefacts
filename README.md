# dotnet-realmikefacts

This repository is a poc for what dotnet mvc can do. 

## Technical Deep Dive

This project was created with Microsoft's mvc template. More information can be found here: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates#web-options

* IAM Solution: Microsoft AzureAD
* Hosting Solution: Microsoft Azure web services
* Database Soluiton: AWS RDS Postrges databse

Edit `appsettings.json` to customize the behavior of OOTB features.

## StyleCop Analyzers
Usage: https://dotnetanalyzers.github.io/StyleCopAnalyzers/

## How to run project
```sh
dotnet run --launch-profile "https"
```

## Documentation

The following documentation is available in this repository.

* `GITHUB.md` - the steps for configuring the application for GitHub actions
* `DEVELOPMENT.md` - provides documentation regarding development of the source code
* `PACKAGING.md` - outlines the automated artifact packaging used by this project
* `STATIC-ANALYSIS.md` - the steps for configuring static code analysis for SonarClou
# dotnet-realmikefacts
