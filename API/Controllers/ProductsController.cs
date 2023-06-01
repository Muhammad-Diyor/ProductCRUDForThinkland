using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromForm] CreateProduct dto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model is not valid");

        try
        {
            var addedProduct = await productService.AddProductAsync(dto);

            return Ok(addedProduct);
        }
        catch (Exception e)
        {
            Log.Error($"Error in GetProduct endpoint. \nException: {e.Message}\nInnerException:{e.InnerException}");

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetProduct([FromRoute] Guid id)
    {
        try
        {
            var product = productService.GetById(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            Log.Error($"Error in GetProduct endpoint. \nException: {e.Message}\nInnerException:{e.InnerException}");

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await productService.GetAllQuestionsAsync();

            return Ok(products);
        }
        catch (Exception e)
        {
            Log.Error($"Error in GetProduct endpoint. \nException: {e.Message}\nInnerException:{e.InnerException}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProduct dto)
    {
        try
        {
            var updatedProduct = await productService.UpdateProductAsync(id, dto);

            return Ok(updatedProduct);
        }
        catch (Exception e)
        {
            Log.Error($"Error in GetProduct endpoint. \nException: {e.Message}\nInnerException:{e.InnerException}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            var deletedProduct = await productService.DeleteByIdAsync(id);

            return Ok(deletedProduct);
        }
        catch (Exception e)
        {
            Log.Error($"Error in GetProduct endpoint. \nException: {e.Message}\nInnerException:{e.InnerException}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
}
