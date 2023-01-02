using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IProductService
    {
        Task DeleteProductByIdAsync(int productId);
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<ProductEntity?> GetProductByIdAsync(int productId);
        Task<int> InsertProductAsync(ProductEntity product);
        Task UpdateProductAsync(ProductEntity product);
    }
}