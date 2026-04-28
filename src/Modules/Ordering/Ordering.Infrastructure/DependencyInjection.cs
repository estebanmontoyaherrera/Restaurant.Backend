using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Interfaces.Persistence;
using Ordering.Application.Interfaces.Services;
using Ordering.Infrastructure.Persistence.Context;
using Ordering.Infrastructure.Persistence.Repositories;
using Ordering.Infrastructure.Services;
using System.Reflection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(OrderingDbContext).Assembly.FullName;

        services.AddDbContext<OrderingDbContext>(
            options => options
            .UseNpgsql(configuration.GetConnectionString("OrderingConnection"), b => b.MigrationsAssembly(assembly)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        var infraAsm = Assembly.GetExecutingAssembly();
        foreach (var impl in infraAsm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
        {
            var @interface = impl.GetInterfaces()
                .FirstOrDefault(i => i.Name == "I" + impl.Name);
            if (@interface != null)
                services.AddScoped(@interface, impl);
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IOrderingQuery, OrderingQuery>();

        services.AddTransient<IExcelService, ExcelService>();
        services.AddTransient<IPdfService, PdfService>();

        return services;
    }
}
