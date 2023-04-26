using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Abstraction;
using ProductManager.BusinessLogic.Services;
using ProductManager.DBMap;
using ProductManager.Repository.RepositoryCollections;

namespace ProductManager.BusinessLogic.ServiceCollections
{
    public static class ServiceCollections
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            //services.AddHostedService<DbInitializer>();

            services.ConfigureRepositories(Configuration);
        }
    }
}