using Identity.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Infrastructure.Persistance;

namespace UserProfile.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UserProfileDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(UserProfileDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public void Update(T entity) => _dbSet.Update(entity);
        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
