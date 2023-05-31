using API.Models;
using API.Models.DTOs;
using API.Models.Entities;

namespace API.Services;

public interface IProductService
{
    Task<Result<Product>> AddProduct(CreateProduct dto);
}
