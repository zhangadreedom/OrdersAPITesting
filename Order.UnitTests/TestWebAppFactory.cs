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
                // 1) 移除生产注册的 DbContext（SQL Server）
                services.RemoveAll<DbContextOptions<OrderDbContext>>();
                services.RemoveAll<OrderDbContext>();

                // 2) 创建一个共享的 In-Memory SQLite 连接（生命周期=整个工厂）
                _conn = new SqliteConnection("Filename=:memory:");
                _conn.Open();

                // 3) 用 SQLite 覆盖注册
                services.AddDbContext<OrderDbContext>(option => option.UseSqlite(_conn!));

                services.AddScoped<IOrderRepository, OrderRepository>();

                // 4) 建表（用 EnsureCreated 即可；若你用迁移，这里可改成 Migrate）
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
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
