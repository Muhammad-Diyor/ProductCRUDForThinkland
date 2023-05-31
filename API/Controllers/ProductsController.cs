using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        var result = await productService.AddProduct(dto);

        return Ok(result);
    }
}
