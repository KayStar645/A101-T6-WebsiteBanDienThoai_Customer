using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SmartPhoneDbContext>
    {
        public SmartPhoneDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SmartPhoneDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SubscribeTopicConnectionString"));

            return new SmartPhoneDbContext(optionsBuilder.Options);
        }
    }
}
