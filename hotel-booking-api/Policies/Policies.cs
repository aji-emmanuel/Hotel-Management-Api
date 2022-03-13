using Microsoft.AspNetCore.Authorization;

namespace hotel_booking_api
{
    public static class Policies
    {
        /// <summary>
        /// Policy for Admin role
        /// </summary>
        public const string Admin = "Admin";
        /// <summary>
        /// Policy for HotelManager role
        /// </summary>
        public const string Manager = "Manager";
        /// <summary>
        /// Policy for a Regular User role
        /// </summary>
        public const string Customer = "Customer";
        /// <summary>
        /// Policy for an Admin and a Hotel Manager
        /// </summary>
        public const string AdminAndManager = "AdminAndHotelManager";
        /// <summary>
        /// policy for a Hotel Manager and a Regular User
        /// </summary>
        public const string ManagerAndCustomer = "HotelManagerAndCustomer";

        /// <summary>
        /// policy for a Admin and a Regular User
        /// </summary>
        public const string AdminAndCustomer = "AdminAndCustomer";
        /// <summary>
        /// Grants Admin User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        /// <summary>
        /// Grants Hotel Managers User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy HotelManagerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Manager).Build();
        }

        /// <summary>
        /// Grants Regular Users User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy CustomerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Customer).Build();
        }

        /// <summary>
        /// Grants Admin and Hotel Managers User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminAndHotelManagerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin, Manager).Build();
        }

        /// <summary>
        /// Grants Hotel Managers and Regular Users User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy HotelManagerAndCustomerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Manager, Customer).Build();
        }

        /// <summary>
        /// Grants Admin and Regular Users User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminAndCustomerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin, Customer).Build();
        }
    }
}
