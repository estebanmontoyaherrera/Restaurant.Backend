using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Ordering.Domain.Entities;
using System.Data;
using System.Reflection;

namespace Ordering.Infrastructure.Persistence.Context;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options, IConfiguration configuration) : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_configuration.GetConnectionString("OrderingConnection"));
}
