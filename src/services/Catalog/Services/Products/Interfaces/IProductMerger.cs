using Common.Models.Products;
using System.Collections.Generic;

namespace Catalog.Services.Products.Interfaces
{
    public interface IProductMerger
    {
        string GetMongoId(int id);
        IEnumerable<Product> MergeProducts(IEnumerable<Product> sqlProducts, IEnumerable<Product> mongoProducts);
        Product MergeProduct(Product sqlProduct, Product mongoProduct);
    }
}
