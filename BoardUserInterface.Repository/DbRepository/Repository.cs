using BoardUserInterfaces.DataAccess.DataBase;
using Microsoft.EntityFrameworkCore;

namespace BoardUserInterface.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BoardUserInterfaceContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(BoardUserInterfaceContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(T entity)
    {
        // Use reflection to get the primary key property of the entity.
        var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
            .Select(x => x.Name).Single();

        // Use reflection again to get the value of the primary key property.
        var keyValue = typeof(T).GetProperty(keyName).GetValue(entity);

        // Find the tracked entity with the same primary key value.
        var existingEntity = await _dbSet.FindAsync(keyValue);
        if (existingEntity != null)
        {
            // Detach the existing tracked entity.
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        // Now attach your updated entity and mark it as modified.
        _context.Entry(entity).State = EntityState.Modified;

        // Save changes to the database.
        await _context.SaveChangesAsync();
    }



    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Log the exception details
            // You can use a logging framework or simply write to the console for debugging purposes
            Console.WriteLine($"An error occurred while saving the entity changes: {ex.InnerException?.Message}");

            // Re-throw the exception to ensure that it gets handled further up the call stack
            throw;
        }
    }


}
