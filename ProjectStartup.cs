using System.Collections.Generic;
using System.Reflection;
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
            builder.Services.AddSingleton<IPasswordProvider, PasswordProvider>();
            
            // AUTOMAPPER
            var assemblies = new List<Assembly>
            {
                typeof(Models.Mappings.Mappings).Assembly,
            };
            builder.Services.AddAutoMapper(assemblies);
        }
    }
}