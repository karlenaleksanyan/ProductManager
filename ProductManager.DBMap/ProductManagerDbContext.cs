using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using ProductManager.DBMap.Models;

namespace ProductManager.DBMap
{
    public class ProductManagerDbContext : DbContext
    {
        #region Models

        public DbSet<Product> Products { get; set; }

        #endregion Models

        #region config

        private static string connectionString;

        public ProductManagerDbContext()
        {
        }

        public ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
            : base(options)
        {
            var extension = options.FindExtension<SqlServerOptionsExtension>();
            connectionString = extension.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        #endregion config
    }
}