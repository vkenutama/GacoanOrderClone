using AutoMapper;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;

namespace Gacoan.Backend.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.TypeName, 
                opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : string.Empty)
            );
        
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        
        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<CreateProductTypeDto, ProductType>();
    }
}