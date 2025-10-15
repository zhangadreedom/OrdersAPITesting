using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Order.Api;
using Order.Domain;
using Order.Infra;
using Order.Infra.Repositories;
using System.Data.Common;

namespace Order.UnitTests
{
    public class TestWebAppFactory : WebApplicationFactory<Program>, IDisposable
    {
        private DbConnection? _conn;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Remove the DbContext registered in production (remove SQL Server, using SQLite for testing)
                services.RemoveAll<DbContextOptions<OrderDbContext>>();
                services.RemoveAll<OrderDbContext>();

                // Create a shared SQLite connection
                _conn = new SqliteConnection("Filename=:memory:");
                _conn.Open();

                services.AddDbContext<OrderDbContext>(option => option.UseSqlite(_conn!));

                services.AddScoped<IOrderRepository, OrderRepository>();

                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                db.Database.EnsureCreated();
            });
        }

        public new void Dispose()
        {
            base.Dispose();
            _conn?.Dispose();
        }
    }
}
