# dotnet-realmikefacts

This repository serves as a IAM demo that leverages OpenAI's training models.

## Technical Deep Dive

This project was created with Microsoft asp.net 8. More information can be found here: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates#web-options

* IAM Solution: Microsoft AzureAD B2C
* Hosting Solution: Microsoft Azure web services
* Database Soluiton: Microsoft Comsos Mongo DB

Edit `appsettings.json` to customize the behavior of OOTB features.

## How to run project
```sh
dotnet build release -c
dotnet run --launch-profile "https"
```

## Documentation

The following documentation is available in this repository.

* `GITHUB.md` - the steps for configuring the application for GitHub actions
* `DEVELOPMENT.md` - provides documentation regarding development of the source code
* `PACKAGING.md` - outlines the automated artifact packaging used by this project
* `STATIC-ANALYSIS.md` - the steps for configuring static code analysis for SonarClou

## RealMikeFacts Live

Please check out RealMikeFacts at https://realmikefacts.azurewebsites.net
