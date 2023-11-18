using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Common;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SmartPhoneDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SmartPhoneConnectionString"))
            );

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Đăng ký ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();

            return services;
        }
    }
}
