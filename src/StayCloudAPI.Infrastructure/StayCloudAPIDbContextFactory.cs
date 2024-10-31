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

            builder.UseSqlServer(
                !string.IsNullOrEmpty(configuration.GetConnectionString("StayCloudDB")) ?
                configuration.GetConnectionString("StayCloudDB") : 
                "Data Source=DESKTOP-C4RFBLF\\SQLEXPRESS;Initial Catalog=StayCloudDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
                );

            return new StayCloudAPIDbContext(builder.Options);
        }
    }
}
