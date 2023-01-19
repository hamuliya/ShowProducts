using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;


namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]


public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }



    [HttpGet]
   
    public async Task<IActionResult> GetProducts()
    {
        //try
        //{
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        //}
        //catch (ResourceNotFoundException ex)
        //{
        //    _logger.LogError(ex, "The requested resource could not be found.");
        //    return NotFound(ex.Message);
        //}
        //catch (UnauthorizedAccessException ex)
        //{
        //    _logger.LogError(ex, "Unauthorized access.");
        //    return Unauthorized(ex.Message);
        //}
        //catch (ValidationException ex)
        //{
        //    _logger.LogError(ex, "Validation Error");
        //    return BadRequest(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "An error occurred while getting all products.");
        //    return StatusCode(500, ex.Message);
        //}
    }




    [HttpGet("{id}", Name = "ProductById")]
    public async Task<IActionResult> GetProduct(int id)
    {
        if (id <= 0)
            return BadRequest("id is invaid.");

        //try
        //{
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product not found.");
            return Ok(product);
        //}
        //catch (ResourceNotFoundException ex)
        //{
        //    _logger.LogError(ex, "The requested resource could not be found.");
        //    return NotFound(ex.Message);
        //}
        //catch (UnauthorizedAccessException ex)
        //{
        //    _logger.LogError(ex, "Unauthorized access.");
        //    return Unauthorized(ex.Message);
        //}
        //catch (ValidationException ex)
        //{
        //    _logger.LogError(ex, "Validation Error");
        //    return BadRequest(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "An error occurred while getting all product.");
        //    return StatusCode(500, ex.Message);
        //}

    }



    [HttpPost]
    public async Task<IActionResult> InsertProduct(ProductEntity product)
    {
        try
        {

            int id = await _productService.InsertProductAsync(product);
            if (id == null) return NotFound("The product was not inserted successfully.");
            return Ok(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while inserting product.");
            return (StatusCode(500, ex.Message));
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductEntity product)
    {
        try
        {
            if (product == null)
                return BadRequest("The product infomation is invaid.");
            await _productService.UpdateProductAsync(product);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while update product.");
            return StatusCode(500, ex.Message);
        }
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id <= 0)
            return BadRequest("id is invalid.");
        try
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete product");
            return StatusCode(500, ex.Message);

        }

    }
}

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException() : base() { }
    public ResourceNotFoundException(string message) : base(message) { }
    public ResourceNotFoundException(string message, Exception inner) : base(message, inner) { }
}







