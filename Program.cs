using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Company.Function;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostingContext, services) => {
        services.Configure<CosmosDBOptions>(hostingContext.Configuration.GetSection(nameof(CosmosDBOptions)));
    })
    .ConfigureAppConfiguration((hostingContext, configBuilder) => 
    {
        if (hostingContext.HostingEnvironment.IsProduction())
        {
            string? keyVaultEndpoint = Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

            if (keyVaultEndpoint is null)
                throw new InvalidOperationException("Store the Key Vault endpoint in a KEYVAULT_ENDPOINT environment variable.");

            var secretClient = new SecretClient(new(keyVaultEndpoint), new DefaultAzureCredential());
            configBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
    })
    .Build();

host.Run();
