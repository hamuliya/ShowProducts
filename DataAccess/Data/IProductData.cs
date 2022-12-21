using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IProductData
    {
        Task DeleteProduct(int productId);
        Task<ProductDBModel?> GetProduct(int productId);
        Task<IEnumerable<ProductDBModel>> GetProducts();
        int InsertProduct(ProductDBModel product);
        Task UpdateProduct(ProductDBModel product);
    }
}