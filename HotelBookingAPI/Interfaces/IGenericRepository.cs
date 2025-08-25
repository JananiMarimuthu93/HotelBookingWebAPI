using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Get all records
        Task<IEnumerable<T>> GetAllAsync();

        // Get by Id
        Task<T?> GetByIdAsync(int id);

        // Add
        Task<T> AddAsync(T entity);

        // Update
        Task<T> UpdateAsync(T entity);

        // Delete
        Task<bool> DeleteAsync(int id);

        IQueryable<T> GetAllQueryable();

        //Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
