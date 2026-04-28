using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Application.Interfaces.Persistence;
using Ordering.Application.Interfaces.Services;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence.Context;
using Ordering.Infrastructure.Persistence.Repositories;
using System.Data;

namespace Ordering.Infrastructure.Services;

public class UnitOfWork(OrderingDbContext context) : IUnitOfWork
{
    private readonly OrderingDbContext _context = context;
    private readonly IGenericRepository<Dish> _dishes = null!;
    private readonly IGenericRepository<Order> _orders = null!;
    private readonly IGenericRepository<OrderDetail> _orderDetails = null!;

    public IGenericRepository<Dish> Dishes => _dishes ?? new GenericRepository<Dish>(_context);
    public IGenericRepository<Order> Orders => _orders ?? new GenericRepository<Order>(_context);
    public IGenericRepository<OrderDetail> OrderDetails => _orderDetails ?? new GenericRepository<OrderDetail>(_context);

    public IDbTransaction BeginTransaction()
    {
        var transaction = _context.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
