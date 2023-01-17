using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
  

    [HttpGet]
    public async Task<IResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
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
            var product = await _productService.GetProductByIdAsync(id);
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

            int id =await  _productService.InsertProductAsync(product);
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
            await _productService.UpdateProductAsync(product);
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
            await _productService.DeleteProductByIdAsync(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
            throw;
        }

    }
}







