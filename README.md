# dotnet-realmikefacts

This repository serves as a IAM demo that leverages OpenAI's training models.

## Technical Deep Dive

This project was created with Microsoft asp.net 8. More information can be found here: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates#web-options

RealMikeFacts leverages the power of AI with OpenAI's powerfully trained models.  More information can be found here: https://platform.openai.com/

* AI Solution: Open AI
* IAM Solution: Microsoft AzureAD B2C
* Hosting Solution: Microsoft Azure web services
* Database Soluiton: Microsoft Comsos Mongo DB

## App configuration
Edit `appsettings.json` to customize the behavior of OOTB features.
```json
{
  "OpenAI": {
    "BaseUrl": "https://api.openai.com/",
    "AccessToken": "...",
    "MaxTokens": 250,
    "temperature": 0.7,
    "model": "..."
  },
  "Twitter": {
    "BaseUrl": "https://api.twitter.com/",
    "ClientId": "...",
    "ClientSecret": "..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AzureAd": {
    "Instance": "https://{domain}.b2clogin.com/",
    "Domain": "{domain}.onmicrosoft.com",
    "ClientId": "...",
    "TenantId": "...",
    "ClientSecret": "...",
    "ClientCertificates": [
    ],
    "SignUpSignInPolicyId": "{name_of_policy_flow}",
    "CallbackPath": "/signin-oidc"
  },
  "Authorization": {
    "Policies": {
      "Users": {
        "RequiredClaims": [
          {
            "ClaimType": "tfp",
            "AllowedValues": [
              "{name_of_policy_flow}"
            ]
          }
        ]
      }
    }
  }
}

```

## How to run project
```sh
dotnet build release -c
dotnet run --launch-profile "https"
```

## How to authorize a new page in the application for a custom group policy.
```csharp
[Authorize(Policy = PolicyNames.AdministratorsGroup)] // Must configure group policy in AzureAD B2C tenant.
public class AdministratorsModel : PageModel
{
    public AdministratorsModel()
    {
    }

    public void OnGet()
    {
    }
}
```

## Documentation

The following documentation is available in this repository.

* `README.md` - the steps for configuring the application.

## RealMikeFacts Live

Please check out RealMikeFacts at https://realmikefacts.azurewebsites.net
