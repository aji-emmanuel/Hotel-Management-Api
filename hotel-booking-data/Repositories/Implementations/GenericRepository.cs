using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<T> _dbSet;
       

        public GenericRepository(HbaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
           

        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
             _dbSet.Update(entity);
        }
    }
}
