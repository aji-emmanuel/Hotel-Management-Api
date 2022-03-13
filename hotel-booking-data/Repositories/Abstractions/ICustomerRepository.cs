using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer GetCustomer(string id);
        Task<Customer> GetCustomerAsync(string id);
        IQueryable<Customer> GetAllUsers();
        Task<IEnumerable<Customer>> GetTopCustomerForManagerAsync(string managerId);
        Task<Customer> GetCustomerDetails(string id);
    }
}