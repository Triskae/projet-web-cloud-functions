using System;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetWeb;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ProjetWeb
{
    public class Startup : FunctionsStartup
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("AppSettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) =>
            {
                var endpoint = Configuration["EndPointUrl"];
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new ArgumentNullException(
                        "Please specify a valid endpoint in the appSettings.json file or your Azure Functions Settings.");
                }

                var authKey = Configuration["AuthorizationKey"];
                if (string.IsNullOrEmpty(authKey) || string.Equals(authKey, "Super secret key"))
                {
                    throw new ArgumentException(
                        "Please specify a valid AuthorizationKey in the appSettings.json file or your Azure Functions Settings.");
                }

                var configurationBuilder = new CosmosClientBuilder(endpoint, authKey);
                return configurationBuilder
                    .Build();
            });
        }
    }
}