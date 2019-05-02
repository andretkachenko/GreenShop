using AutoMapper;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Service.Categories;

namespace GreenShop.Catalog.Api.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
