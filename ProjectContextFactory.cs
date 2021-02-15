using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using projet_web.DatabaseContext;

namespace projet_web
{
    public class ProjectContextFactory : IDesignTimeDbContextFactory<ProjectContext>
    {
        public ProjectContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
            optionsBuilder.UseCosmos("https://administrator.documents.azure.com:443/",
                "ChGvETWDYQNic5koOjbfDvOs1dDXSe1VPOwftlWDSvxEIf4DQuyGApjv1QWIMlSCfWrXfy3qDHyd9is0jWSWVg==",
                "administrator");

            return new ProjectContext(optionsBuilder.Options);
        }
    }
}