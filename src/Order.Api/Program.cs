using FluentValidation;
using FluentValidation.AspNetCore;
using Order.Application.Services;
using Order.Application.Validation;
using Order.Infra.DI;

namespace Order.Api;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 内置 OpenAPI（json/yaml 文档，没有 UI）
        builder.Services.AddOpenApi();

        builder.Services.AddControllers();

        // 应用服务 & 验证
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderValidator).Assembly);

        // 基础设施
        var connectionString = builder.Configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("ConnectionString 'Default' is missing.");

        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddInfrastructure(connectionString);
        }

        var app = builder.Build();

        // 开发环境暴露 OpenAPI 文档（默认 /openapi/v1.json）
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // 非测试环境启用 HTTPS 重定向
        if (!app.Environment.IsEnvironment("Testing"))
        {
            app.UseHttpsRedirection();
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Order.Infra.OrderDbContext>();
            db.Database.EnsureCreated();
        }

        app.MapControllers();
        app.Run();
    }
}