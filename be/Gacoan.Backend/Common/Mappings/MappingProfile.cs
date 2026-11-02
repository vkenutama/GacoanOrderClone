using AutoMapper;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;

namespace Gacoan.Backend.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1. Product
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.TypeName, 
                opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : string.Empty)
            );
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        
        // 2. ProductType (Category)
        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<CreateProductTypeDto, ProductType>();

        // 3. Restaurant & OperationalHour
        CreateMap<Restaurant, RestaurantDto>();
        CreateMap<CreateRestaurantDto, Restaurant>();
        CreateMap<UpdateRestaurantDto, Restaurant>();

        CreateMap<OperationalHour, OperationalHourDto>();
        CreateMap<CreateOperationalHourDto, OperationalHour>();

        // 4. Cart & OrderItem
        CreateMap<OrderItem, CartItemDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            );

        CreateMap<OrderCart, CartDto>()
            .ForMember(
                dest => dest.RestaurantName,
                opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.Name : string.Empty)
            )
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems)
            )
            .ForMember(
                dest => dest.TotalAmount,
                opt => opt.MapFrom(src => src.OrderItems.Sum(i => i.UnitPrice * i.Quantity))
            );
    }
}
