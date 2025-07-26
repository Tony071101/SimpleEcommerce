using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface IAddressService
    {
        // Admin: GetAll, GetById (for any customer)
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address?> GetAddressByIdAsync(Guid id);
        Task<IEnumerable<Address>> GetAddressesByCustomerIdAsync(Guid userId); // For admin to view all of a user's addresses

        // Admin & Customer: Add, Update, Delete
        Task<Address> AddAddressAsync(Guid userId, Address address); // CustomerID is in DTO
        Task<Address?> UpdateAddressAsync(Guid id, Guid userId, Address address); // CustomerID is in DTO for validation/ownership check
        Task<bool> DeleteAddressAsync(Guid id, Guid userId);
    }
}