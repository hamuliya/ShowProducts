using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class ProductData : IProductData
{
    private readonly ISqlDataAccess _db;
    public ProductData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<ProductDBModel>> GetProducts() =>
        _db.LoadData<ProductDBModel, dynamic>("dbo.spProduct_GetAll", new { });

    public async Task<ProductDBModel?> GetProduct(int productId)
    {
        var results = await _db.LoadData<ProductDBModel, dynamic>
            ("dbo.spProduct_GetById", new { ProductId = productId });
        return results.FirstOrDefault();

    }

    public int InsertProduct(ProductDBModel product)
    {
        int id = _db.SaveData("dbo.spProduct_Insert", new { product.ProductId, product.Title, product.UploadDate, product.Detail });
        return id;
    }

    public Task UpdateProduct(ProductDBModel product) =>
        _db.ExecData("dbo.spProduct_Update", product);

    public Task DeleteProduct(int productId) =>
        _db.ExecData("dbo.spProduct_Delete", new { ProductId = productId });

}
