using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain;
using Order.Infra.Repositories;

namespace Order.Infra.DI
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            
            services.AddDbContext<OrderDbContext>(opt => opt.UseSqlServer(connectionString));
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
