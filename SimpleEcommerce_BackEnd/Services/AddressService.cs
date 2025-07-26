using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext dbContext;

        public AddressService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region AdminOnly
        public async Task<IEnumerable<Address>> GetAllAddressesAsync() => await dbContext.Addresses.ToListAsync();

        public async Task<Address?> GetAddressByIdAsync(Guid id) => await dbContext.Addresses.FindAsync(id);

        public async Task<IEnumerable<Address>> GetAddressesByCustomerIdAsync(Guid userId)
        {
            return await dbContext.Addresses
                                   .Where(a => a.UserID == userId)
                                   .ToListAsync();
        }
        #endregion
        
        #region Admin & Customer
        public async Task<Address> AddAddressAsync(Guid userId, Address address)
        {
            address.UserID = userId;
            if (address.AddressID == Guid.Empty)
            {
                address.AddressID = Guid.NewGuid(); // Đảm bảo có ID mới nếu chưa có
            }
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> UpdateAddressAsync(Guid id, Guid userId, Address address)
        {
            var existingAddress = await dbContext.Addresses.FindAsync(id);

            if (existingAddress == null)
            {
                return null; // Address not found
            }

            // Kiểm tra quyền sở hữu trong service
            // Chỉ người dùng sở hữu hoặc Admin mới có quyền cập nhật
            if (existingAddress.UserID != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this address.");
            }

            existingAddress.Line1 = address.Line1;
            existingAddress.City = address.City;
            existingAddress.Country = address.Country;
            // Cập nhật các thuộc tính khác nếu có

            await dbContext.SaveChangesAsync();
            return existingAddress;
        }

        public async Task<bool> DeleteAddressAsync(Guid id, Guid userId)
        {
            var addressToDelete = await dbContext.Addresses.FindAsync(id);

            if (addressToDelete == null)
            {
                return false; // Address not found
            }

            // Kiểm tra quyền sở hữu trong service
            // Chỉ người dùng sở hữu hoặc Admin mới có quyền xóa
            if (addressToDelete.UserID != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this address.");
            }

            dbContext.Addresses.Remove(addressToDelete);
            await dbContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}