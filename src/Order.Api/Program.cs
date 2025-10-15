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

        builder.Services.AddOpenApi();

        builder.Services.AddControllers();

        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderValidator).Assembly);

        // infrastructure
        var connectionString = builder.Configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("ConnectionString 'Default' is missing.");

        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddInfrastructure(connectionString);
        }

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Non-testing environments enable HTTPS redirection and ensure database is created
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