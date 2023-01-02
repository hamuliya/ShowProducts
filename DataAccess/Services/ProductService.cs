using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class ProductService : IProductService
{
    private readonly ISqlDataAccess _db;
    public ProductService(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync() =>
       await _db.loadDataAsync<ProductEntity, dynamic>("dbo.spProduct_GetAll", new { });

    public async Task<ProductEntity?> GetProductByIdAsync(int productId)
    {
        var results = await _db.loadDataAsync<ProductEntity, dynamic>
            ("dbo.spProduct_GetById", new { productId = productId });
        if (results is null) return null;
        return results.FirstOrDefault();

    }

    public async Task<int> InsertProductAsync(ProductEntity product)
    {
        int id = await _db.saveDataAsync("dbo.spProduct_Insert", new { product.productId, product.title, product.uploadDate, product.detail });
        return id;
    }

    public async Task UpdateProductAsync(ProductEntity product) =>
        await _db.execDataAsync("dbo.spProduct_Update", product);

    public async Task DeleteProductByIdAsync(int productId) =>
        await _db.execDataAsync("dbo.spProduct_Delete", new { productId = productId });

}
