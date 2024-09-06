using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Interface
{
    public interface IBaseRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> ListAllAsync();
        Task<IEnumerable<T>> ListIEnumerableAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetFirstOrDefaultAsync();

    }
}
