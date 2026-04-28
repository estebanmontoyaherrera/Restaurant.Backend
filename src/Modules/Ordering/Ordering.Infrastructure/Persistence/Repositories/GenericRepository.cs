using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces.Persistence;
using Ordering.Infrastructure.Persistence.Context;
using SharedKernel.Primitive;

namespace Ordering.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(OrderingDbContext context) : IGenericRepository<T> where T : StateAuditableEntity
{
    private readonly OrderingDbContext _context = context;
    private readonly DbSet<T> _entity = context.Set<T>();

    public IQueryable<T> GetAllQueryable()
    {
        var response = _entity
           .Where(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);

        return response;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var response = await _entity
            .Where(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
            .ToListAsync();

        return response;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var response = await _entity
            .SingleOrDefaultAsync(x => x.Id == id && x.AuditDeleteUser == null && x.AuditDeleteDate == null);

        return response!;
    }

    public async Task CreateAsync(T entity)
    {
        entity.AuditCreateUser = 1;
        entity.AuditCreateDate = DateTime.UtcNow;

        await _context.AddAsync(entity);
    }

    public void UpdateAsync(T entity)
    {
        entity.AuditUpdateUser = 1;
        entity.AuditUpdateDate = DateTime.UtcNow;

        _context.Update(entity);

        _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
        _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;
    }

    public async Task DeleteAsync(int id)
    {
        T entity = await GetByIdAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"Entity with id {id} was not found.");

        entity.AuditDeleteUser = 1;
        entity.AuditDeleteDate = DateTime.UtcNow;

        _context.Update(entity);
    }
}
