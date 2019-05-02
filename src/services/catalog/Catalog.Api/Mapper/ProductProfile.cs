using AutoMapper;
using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Service.Products;

namespace GreenShop.Catalog.Api.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Specification, SpecificationDto>();
        }
    }
}
