using Blog.Data;
using Blog.Data.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blog.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _context;
        public DbSet<T> Set { get; private set; }
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            var set = _context.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync<T>(entity);

            //var entry = _context.Entry(entity);
            //if (entry.State == EntityState.Detached)
            //    await entry.Context.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                entry.Context.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            var entry = _context.Entry(entity);
            entry.Context.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToArrayAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
