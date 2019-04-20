using AutoMapper;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Service.Products;

namespace GreenShop.Catalog.Mapper
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
