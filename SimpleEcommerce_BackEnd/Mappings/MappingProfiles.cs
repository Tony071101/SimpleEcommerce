using AutoMapper;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Models.Dtos.Address;
using SimpleEcommerce_BackEnd.Models.Dtos.Cart;
using SimpleEcommerce_BackEnd.Models.Dtos.Category;
using SimpleEcommerce_BackEnd.Models.Dtos.Order;
using SimpleEcommerce_BackEnd.Models.Dtos.Product;
using SimpleEcommerce_BackEnd.Models.Dtos.User;
using SimpleEcommerce_BackEnd.Models.Dtos.Auth;
using SimpleEcommerce_BackEnd.Models.Dtos.Talent;

namespace SimpleEcommerce_BackEnd.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // --- User Mappings ---
            // Map User Entity to UserDto (for responses)
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)) // Ensure PhoneNumber mapping
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are handled by UserService manually

            // Map RegisterDto to User Entity (for creating new users)
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)) // Ensure PhoneNumber mapping
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            // Map UserUpdateDto to User Entity (for updating existing users by Admin)
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)) // Ensure PhoneNumber mapping
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID will be used for lookup
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore()) // These are not updated via DTO
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password/Roles handled by UserManager/Service

            // --- Address Mappings ---
            CreateMap<CreateAddressDto, Address>()
                .ForMember(dest => dest.AddressID, opt => opt.Ignore()) // ID sẽ được tạo trong Service
                .ForMember(dest => dest.UserID, opt => opt.Ignore());   // UserID sẽ được gán trong Service/Controller
            CreateMap<UpdateAddressDto, Address>()
                .ForMember(dest => dest.AddressID, opt => opt.Ignore()) // ID không được cập nhật từ DTO
                .ForMember(dest => dest.UserID, opt => opt.Ignore());   // UserID không được cập nhật từ DTO
            CreateMap<Address, AddressResponseDto>();

            // --- Category Mappings ---
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.CategoryID, opt => opt.Ignore()); // CategoryID sẽ được tạo trong service
            CreateMap<Category, CategoryResponseDto>();

            // --- Talent Mappings ---
            CreateMap<CreateTalentDto, Talent>()
                .ForMember(dest => dest.TalentID, opt => opt.Ignore()); // TalentID sẽ được tạo trong service
            CreateMap<Talent, TalentResponseDto>();

            // --- Product Mappings ---
            CreateMap<CreateProductRequestDto, Product>()
                .ForMember(dest => dest.ProductID, opt => opt.Ignore()); // ProductID sẽ được tạo trong Controller/Service

            // Ánh xạ từ Product Entity sang ProductResponseDto (cho response)
            // Đảm bảo Category và Talent đã được eager load trong Service
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TalentName, opt => opt.MapFrom(src => src.Talent.TalentName));

            // --- Cart Mappings ---
            CreateMap<CartItemRequestDto, Cart>()
                .ForMember(dest => dest.CartID, opt => opt.Ignore()) // ID sẽ được tạo trong service
                .ForMember(dest => dest.UserID, opt => opt.Ignore()); // UserID sẽ được gán trong service

            // Mapping từ Cart Entity sang CartItemResponseDto (output)
            CreateMap<Cart, CartItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ProductImageUrl))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.ProductPrice));
            // --- Order and OrderDetail Mappings ---
            // Mapping từ CreateOrderRequestDto sang Order Entity (cho đầu vào)
            CreateMap<CreateOrderRequestDto, Order>()
                .ForMember(dest => dest.OrderID, opt => opt.Ignore()) // ID sẽ được tạo trong service
                .ForMember(dest => dest.UserID, opt => opt.Ignore()) // UserID sẽ được lấy từ token
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore()) // Ngày sẽ được đặt trong service
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Tổng tiền sẽ được tính trong service
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Trạng thái sẽ được đặt trong service
                .ForMember(dest => dest.User, opt => opt.Ignore()) // User Entity sẽ không được ánh xạ trực tiếp từ DTO
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.Items)); // Ánh xạ Items sang OrderDetails

            // Mapping từ CreateOrderItemRequestDto sang OrderDetail Entity (cho đầu vào chi tiết)
            CreateMap<CreateOrderItemRequestDto, OrderDetail>()
                .ForMember(dest => dest.OrderDetailID, opt => opt.Ignore()) // ID sẽ được gán trong Service
                .ForMember(dest => dest.OrderID, opt => opt.Ignore()) // OrderID sẽ được gán trong Service
                .ForMember(dest => dest.Order, opt => opt.Ignore()) // Order Entity sẽ không được ánh xạ trực tiếp từ DTO
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Giá sẽ được lấy từ DB trong Service
                .ForMember(dest => dest.Product, opt => opt.Ignore()); // Product Entity sẽ không được ánh xạ trực tiếp từ DTO

            // Mapping từ Order Entity sang OrderResponseDto (cho đầu ra)
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName)) // Lấy Username từ User Entity
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderDetails)); // Ánh xạ OrderDetails sang Items

            // Mapping từ OrderDetail Entity sang OrderDetailDto (cho đầu ra chi tiết của từng mặt hàng)
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price)) // Giá đã được đặt trong service
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            // Mapping từ UpdateOrderStatusDto sang string (cho service)
            CreateMap<UpdateOrderStatusDto, string>()
                .ConvertUsing(src => src.NewStatus);
        }
    }
}