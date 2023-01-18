using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService,ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }
  

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all products.");
            return (StatusCode(500,ex.Message));
        }
    }





    [HttpGet("{id}", Name = "ProductById")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return Results.NotFound();
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting Product");
            return (StatusCode(500,ex.Message));
        }
    }



    [HttpPost]
    public async Task<IActionResult> InsertProduct(ProductEntity product)
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
    public async Task<IActionResult> UpdateProduct(ProductEntity product)
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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete product");
            return StatusCode(500,ex.Message);
            throw;
        }

    }
}







