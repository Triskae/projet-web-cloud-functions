using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ProjetWeb.Auth;


[assembly: FunctionsStartup(typeof(ProjetWeb.ProjectStartup))]
namespace ProjetWeb
{
    public class ProjectStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<TokenIssuer>();
        }
    }
}