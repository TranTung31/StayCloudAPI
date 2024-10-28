using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StayCloudAPI.Infrastructure
{
    public class StayCloudAPIDbContextFactory : IDesignTimeDbContextFactory<StayCloudAPIDbContext>
    {
        public StayCloudAPIDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<StayCloudAPIDbContext>();

            builder.UseSqlServer(configuration.GetConnectionString("StayCloudDB"));

            return new StayCloudAPIDbContext(builder.Options);
        }
    }
}
