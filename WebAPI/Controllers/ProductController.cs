using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productData;

    public ProductController(IProductService productData)
    {
        _productData = productData;
    }
  

    [HttpGet]
    public async Task<IResult> GetProducts()
    {
        try
        {
            var products = await _productData.GetAllProductsAsync();
            return Results.Ok(products);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }





    [HttpGet("{id}", Name = "ProductById")]
    public async Task<IResult> GetProduct(int id)
    {
        try
        {
            var product = await _productData.GetProductByIdAsync(id);
            if (product == null) return Results.NotFound();
            return Results.Ok(product);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IResult> InsertProduct(ProductEntity product)
    {
        try
        {

            int id =await  _productData.InsertProductAsync(product);
            return Results.Ok(id);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }

    [HttpPut]
    public async Task<IResult> UpdateProduct(ProductEntity product)
    {
        try
        {
            await _productData.UpdateProduct(product);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    [HttpDelete]
    public async Task<IResult> DeleteProduct(int id)
    {
        try
        {
            await _productData.DeleteProduct(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
            throw;
        }

    }
}







