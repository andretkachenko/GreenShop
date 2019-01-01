using Common.Configuration.MongoDB;
using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Products;
using Dapper;
using Dapper.Contrib.Extensions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Properties;
using AutoMapper;
using System.Linq;

namespace Catalog.DataAccessors
{
    public class Products : ISqlDataAccessor<Product>
    {
        private readonly ISqlContext _sql;
        private readonly IMongoContext _mongoContext;
        private readonly IMapper _mapper;

        public Products(ISqlContext sqlContext, IMongoContext mongoContext, IMapper mapper)
        {
            _sql = sqlContext;
            _mongoContext = mongoContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAll()
        {
            IEnumerable<Product> sqlProducts, mongoProducts, products;

            using (var context = _sql.Context)
            {
                sqlProducts = await context.GetAllAsync<Product>();
            }

            mongoProducts = await _mongoContext.Database.GetCollection<Product>(Resources.Products).Find(_ => true).ToListAsync();
            products = MergeProducts(sqlProducts, mongoProducts);

            return products;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> Get(int id)
        {
            using (var context = _sql.Context)
            {
                var category = await context.GetAsync<Product>(id);

                return category;
            }
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Task with specified Category</returns>
        public async Task<int> Add(Product product)
        {
            using (var context = _sql.Context)
            {
                var id = await context.InsertAsync(product);

                return id;
            }
        }

        /// <summary>
        /// Asynchronously removed Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Delete(int id)
        {
            using (var context = _sql.Context)
            {
                var affectedRows = await context.ExecuteAsync(@"
                    DELETE
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                });

                return affectedRows;
            }
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Edit(Product product)
        {
            using (var context = _sql.Context)
            {
                var query = @"
                    UPDATE [Products]
                    SET
                ";

                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    query += " [Name] = @name";
                }
                if (product.CategoryId != 0)
                {
                    query += " [CategoryId] = @categoryId";
                }
                if (!string.IsNullOrWhiteSpace(product.Description))
                {
                    query += " [Description] = @description";
                }
                if (product.BasePrice != 0)
                {
                    query += " [BasePrice] = @basePrice";
                }
                if (product.Rating != 0)
                {
                    query += " [Rating] = @rating";
                }

                query += " WHERE [Id] = @id";

                var affectedRows = await context.ExecuteAsync(query, new
                {
                    id = product.Id,
                    name = product.Name,
                    parentId = product.CategoryId,
                    description = product.Description,
                    basePrice = product.BasePrice,
                    rating = product.Rating
                });

                return affectedRows;
            }
        }

        private IEnumerable<Product> MergeProducts(IEnumerable<Product> sqlProducts, IEnumerable<Product> mongoProducts)
        {
            var products = new List<Product>();

            foreach (var sqlProduct in sqlProducts)
            {
                var mongoProduct = mongoProducts.FirstOrDefault(x => x.Id == sqlProduct.Id);
                if (mongoProduct != null)
                {
                    _mapper.Map(sqlProduct, mongoProduct);
                }
                products.Add(sqlProduct);
            }

            return products;
        }
    }
}
