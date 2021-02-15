using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using projet_web;
using projet_web.DatabaseContext;

[assembly: WebJobsStartup(typeof(StartUp))]

namespace projet_web
{
    class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseCosmos("https://administrator.documents.azure.com:443/",
                    "ChGvETWDYQNic5koOjbfDvOs1dDXSe1VPOwftlWDSvxEIf4DQuyGApjv1QWIMlSCfWrXfy3qDHyd9is0jWSWVg==",
                    "administrator"));
        }
    }
}