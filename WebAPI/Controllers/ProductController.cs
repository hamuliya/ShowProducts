using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductData _productData;

    public ProductController(IProductData productData)
    {
        _productData = productData;
    }
  

    [HttpGet]
    public async Task<IResult> GetProducts()
    {
        try
        {
            var products = await _productData.GetProducts();
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
            var product = await _productData.GetProduct(id);
            if (product == null) return Results.NotFound();
            return Results.Ok(product);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }

    [HttpPost]
    public IResult InsertProduct(ProductDBModel product)
    {
        try
        {

            int id =  _productData.InsertProduct(product);
            return Results.Ok(id);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }

    [HttpPut]
    public async Task<IResult> UpdateProduct(ProductDBModel product)
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







