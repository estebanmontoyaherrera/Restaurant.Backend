using Ordering.Application.Interfaces.Persistence;
using Ordering.Domain.Entities;
using System.Data;

namespace Ordering.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Dish> Dishes { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<OrderDetail> OrderDetails { get; }
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
