using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.DBMap;
using ProductManager.Repository.Abstraction;

namespace ProductManager.Repository.RepositoryCollections
{
    public static class ServiceCollections
    {
        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ProductManagerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductManagerConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}