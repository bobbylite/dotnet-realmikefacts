# **RealMikeFacts - AI-Powered IAM Demo**

Welcome to **RealMikeFacts**, an innovative Identity and Access Management (IAM) demo powered by **OpenAI**. This project integrates **Microsoft AzureAD B2C** and leverages the latest in **AI** to deliver powerful functionality in a modern **ASP.NET Core 8** application, hosted on **Azure Web Apps**.

## 🚀 **Technical Overview**

This demo app was built with cutting-edge tools and technologies. Here's a brief overview:

- **AI Solution**: [OpenAI](https://platform.openai.com) — Utilize state-of-the-art AI models for various intelligent features.
- **IAM Solution**: [Azure AD B2C](https://learn.microsoft.com/en-us/azure/active-directory-b2c/overview) — Secure identity management for users.
- **Hosting Solution**: [Azure Web Apps](https://learn.microsoft.com/en-us/azure/app-service/) — Scalable cloud hosting for web applications.
- **Database Solution**: [Azure Cosmos DB (Mongo API)](https://learn.microsoft.com/en-us/azure/cosmos-db/mongodb/) — NoSQL database for high-performance data storage.
- **Twitter Integration**: Twitter API integration for automated social media interactions.

## 🛠 **Configuration Guide**

To configure the application, edit the necessary settings to suit your environment and adjust the behavior of the application accordingly.

## App configuration
Edit `appsettings.json` to customize the behavior of OOTB features.
```json
{
  "OpenAI": {
    "BaseUrl": "https://api.openai.com/",
    "AccessToken": "...",
    "MaxTokens": 250,
    "temperature": 0.7,
    "model": "gpt-3.5-turbo"
  },
  "Twitter": {
    "BaseUrl": "https://api.twitter.com/",
    "ClientId": "...",
    "ClientSecret": "...",
    "RefreshToken": "...",
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Identity": "Information"
    }
  },
  "AllowedHosts": "*",
  "AzureAd": {
    "Instance": "https://example.b2clogin.com/",
    "Domain": "example.onmicrosoft.com",
    "ClientId": "...",
    "TenantId": "...",
    "ClientSecret": "...",
    "ClientCertificates": [
    ],
    "SignUpSignInPolicyId": "B2C__signup_signin_flow",
    "CallbackPath": "/signin-oidc"
  },
  "Authorization": {
    "Policies": {
      "Users": {
        "RequiredClaims": [
          {
            "ClaimType": "tfp",
            "AllowedValues": [
              "B2C_signup_signin_flow"
            ]
          }
        ]
      }
    }
  }
}
```

### Customization Highlights:
- **OpenAI Settings**: Adjust the API token, max tokens, and model parameters for optimal AI responses.
- **Azure AD B2C Policies**: Customize Azure AD authentication and group policies for user roles and claims.

## 💻 **Running the Project**

Follow these simple steps to get started with the application:

1. Build the project in release mode:
    ```bash
    dotnet build -c Release
    ```
2. Run the application with HTTPS enabled:
    ```bash
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

## 🐦 **Twitter API Integration**

The application integrates with the **Twitter API** to enable seamless interaction with Twitter. To authorize and obtain an **Access Token**, the app uses the **OAuth 2.0 Authorization Code Grant** flow.

### How it works:
1. The application first redirects the user to Twitter's authorization page, where they can grant access to their Twitter account.
2. After authorization, Twitter sends back an **Authorization Code**.
3. The application exchanges this code for an **Access Token** and a **Refresh Token**.
4. The **Refresh Token** is stored securely and used to request new **Access Tokens** when the current one expires, without needing user intervention again.

### Why use the Refresh Token?
The **Access Token** has a limited lifespan. By using a **Refresh Token**, the application can renew the **Access Token** without requiring users to re-authenticate each time. This is critical for long-term integrations where automated tasks (like posting tweets or retrieving user data) need to be performed regularly.

For detailed documentation on how to implement this, refer to [Twitter OAuth 2.0 - User Access Token](https://developer.x.com/en/docs/authentication/oauth-2-0/user-access-token).

## 📚 **Documentation**

Key documentation resources available in this repository:

- **`README.md`** — Step-by-step guide for configuration and deployment.

## 🌐 **Live Demo**

Explore the live version of **RealMikeFacts** hosted on Azure:  
🔗 [RealMikeFacts Live](https://realmikefacts.azurewebsites.net)