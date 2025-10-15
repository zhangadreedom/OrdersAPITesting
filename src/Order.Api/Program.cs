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

        // ���� OpenAPI��json/yaml �ĵ���û�� UI��
        builder.Services.AddOpenApi();

        builder.Services.AddControllers();

        // Ӧ�÷��� & ��֤
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderValidator).Assembly);

        // ������ʩ
        var connectionString = builder.Configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("ConnectionString 'Default' is missing.");

        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddInfrastructure(connectionString);
        }

        var app = builder.Build();

        // ����������¶ OpenAPI �ĵ���Ĭ�� /openapi/v1.json��
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // �ǲ��Ի������� HTTPS �ض���
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