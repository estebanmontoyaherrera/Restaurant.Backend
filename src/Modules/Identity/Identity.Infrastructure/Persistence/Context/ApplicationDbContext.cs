using Identity.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Reflection;

namespace Identity.Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<User> Users { get; set; } 
    public DbSet<Role> Roles { get; set; } 
    public DbSet<RolePermission> RolePermissions { get; set; } 
    public DbSet<Permission> Permissions { get; set; } 
    public DbSet<Menu> Menus { get; set; } 
    public DbSet<MenuRole> MenuRoles { get; set; } 
    public DbSet<UserRole> UserRoles { get; set; } 
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public IDbConnection CreateConnection() => new NpgsqlConnection(_configuration.GetConnectionString("IdentityConnection"));
}
