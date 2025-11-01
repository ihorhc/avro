---
applyTo: "**/*.cs,**/*.json"
---

# Secrets Management

## Guidelines
- **Never commit secrets** to source control:
  ```csharp
  // ❌ NEVER do this
  var apiKey = "sk-1234567890abcdef";
  var connString = "Server=prod;Database=Avro;User=sa;Password=P@ssw0rd";
  ```

- **Use User Secrets** for local development:
  ```bash
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=..."
  ```

- **Use Azure Key Vault** for production:
  ```csharp
  builder.Configuration.AddAzureKeyVault(
      new Uri($"https://{keyVaultName}.vault.azure.net/"),
      new DefaultAzureCredential());
  
  // Access secrets
  var apiKey = builder.Configuration["ApiKey"];
  ```

- **Use environment variables** as fallback:
  ```csharp
  var apiKey = builder.Configuration["ApiKey"] 
      ?? Environment.GetEnvironmentVariable("API_KEY");
  ```

- **Rotate secrets regularly** and use managed identities:
  ```csharp
  services.AddDbContext<AppDbContext>(options =>
  {
      var connString = builder.Configuration.GetConnectionString("DefaultConnection");
      options.UseSqlServer(connString, sqlOptions =>
      {
          sqlOptions.EnableRetryOnFailure();
          sqlOptions.UseAzureSqlDefaults(); // Uses managed identity
      });
  });
  ```

- **Add secrets files to .gitignore**:
  ```gitignore
  appsettings.Development.json
  appsettings.*.json
  secrets.json
  .env
  *.pfx
  *.key
  ```

## Anti-Patterns
- ❌ Never hardcode credentials in code
- ❌ Don't commit appsettings.Production.json
- ❌ Avoid logging sensitive data
- ❌ Never store secrets in environment variables in production (use Key Vault)