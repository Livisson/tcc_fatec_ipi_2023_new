using GestaoComercio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetByIdAsync(int? id);
        Task<T> CreateAsync(T name);
        Task<T> UpdateAsync(T name);
        Task<T> RemoveAsync(T name);
        T Get(Func<T, bool> expression);
        IEnumerable<T> GetAll(Func<T, bool> expression);
        Task<int> Save();
    }
}
