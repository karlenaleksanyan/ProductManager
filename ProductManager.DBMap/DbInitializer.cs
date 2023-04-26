using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductManager.Models;

namespace ProductManager.DBMap
{
    public class DbInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<OstupultHub> _hubContext;

        public DbInitializer(IServiceProvider serviceProvider, IHubContext<OstupultHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ProductManagerDbContext>();

                // Apply any pending migrations to the database
                await dbContext.Database.MigrateAsync();

                await InsertRecord(dbContext);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Perform any cleanup here
            return Task.CompletedTask;
        }

        private async Task InsertRecord(ProductManagerDbContext dbContext)
        {
            try
            {
                int numberOfProducts = 50000;
                if (dbContext.Products.Count() < numberOfProducts)
                {
                    var rnd = new Random();
                    // SQL command to insert a product
                    string sqlCommand = "INSERT INTO Products ( Name, Price, Barcode, PLU, IsActive, CreateDate) " +
                        "VALUES ( @Name, @Price, @Barcode, @PLU, @IsActive, @CreateDate)";

                    for (int i = dbContext.Products.Count() + 1; i <= numberOfProducts; i++)
                    {
                        var Barcode = rnd.Next().ToString("0000000000000");
                        var PLU = rnd.Next(1, 100000);

                        //TimeOut
                        if (await dbContext.Products.FirstOrDefaultAsync(x => x.Barcode == Barcode) != null)
                        {
                            Barcode = null;
                        }

                        while (await dbContext.Products.FirstOrDefaultAsync(x => x.PLU == PLU) != null)
                        {
                            PLU++;
                        }

                        var parameters = new List<SqlParameter>
                                                {
                                                    new SqlParameter("@Name", $"Product{i}"),
                                                    new SqlParameter("@Price", rnd.Next(1, 500) * 10),
                                                    new SqlParameter("@Barcode", Barcode),
                                                    new SqlParameter("@PLU", PLU),
                                                    new SqlParameter("@IsActive", 1),
                                                    new SqlParameter("@CreateDate", DateTime.UtcNow)
                                                };

                        // Execute the query with parameters
                        var result = await dbContext.Database.ExecuteSqlRawAsync(sqlCommand, parameters);

                        await _hubContext.Clients.All.SendAsync("progressInsertRecord", result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Send an error response to the client
                await _hubContext.Clients.All.SendAsync("InsertError", ex.Message);
            }
        }
    }
}