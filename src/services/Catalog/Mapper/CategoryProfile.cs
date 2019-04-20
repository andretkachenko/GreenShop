using AutoMapper;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Service.Categories;

namespace GreenShop.Catalog.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
