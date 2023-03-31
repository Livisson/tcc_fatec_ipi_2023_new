using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Infra.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context = null;
        private DbSet<T> _table = null;

        public GenericRepository(ApplicationDbContext _context)
        {
            this._context = _context;
            _table = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T name)
        {
            _context.Add(name);
            await _context.SaveChangesAsync();
            return name;
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _table.AsNoTracking().ToListAsync();
        }

        public async Task<T> RemoveAsync(T name)
        {
            _context.Remove(name);
            await _context.SaveChangesAsync();
            return name;
        }

        public async Task<T> UpdateAsync(T name)
        {
            _context.Entry(name).State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            await _context.SaveChangesAsync();
            //_table.Attach(name);
            //_context.Entry(name).State = EntityState.Modified;
            _context.Update(name);
            await _context.SaveChangesAsync();
            return name;
        }

        public T Get(Func<T, bool> expression)
        {
            return _table.AsNoTracking().ToListAsync().Result.Where(expression).FirstOrDefault();
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll(Func<T, bool> expression)
        {
            return _table.AsNoTracking().ToListAsync().Result.Where(expression);
        }
    }
}
