using AutoMapper;
using Common.Models.Products;

namespace Catalog.Utils.Mappings
{
    public class ProductsMergeProfile : Profile
    {
        public ProductsMergeProfile()
        {
            CreateMap<Product, Product>().ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
