﻿using DataAccess.DbAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services;

public class ProductService : IProductService
{
    private readonly ISqlDataAccess _db;
    public ProductService(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync() =>
       await _db.LoadDataAsync<ProductEntity, dynamic>("dbo.spProduct_GetAll", new { });

    public async Task<ProductEntity?> GetProductByIdAsync(int productId)
    {
        var results = await _db.LoadDataAsync<ProductEntity, dynamic>
            ("dbo.spProduct_GetById", new { ProductId = productId });
        if (results is null) return null;
        return results.FirstOrDefault();

    }

    public async Task<int> InsertProductAsync(ProductEntity product)
    {
        int id = await _db.SaveDataAsync("dbo.spProduct_Insert", new { product.ProductId, product.Title, product.UploadDate, product.Detail });
        return id;
    }

    public async Task UpdateProductAsync(ProductEntity product) =>
        await _db.ExecDataAsync("dbo.spProduct_Update", product);

    public async Task DeleteProductByIdAsync(int productId) =>
        await _db.ExecDataAsync("dbo.spProduct_Delete", new { ProductId = productId });

}
