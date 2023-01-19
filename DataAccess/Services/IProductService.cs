using DataAccess.Entities;

namespace DataAccess.Services
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