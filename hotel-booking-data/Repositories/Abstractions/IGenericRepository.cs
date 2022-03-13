using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
