using API.Models;
using API.Models.DTOs;
using API.Models.Entities;

namespace API.Services;

public interface IProductService
{
    Task<Result<Product>> AddProductAsync(CreateProduct dto);
    Task<Result<List<Product>>> GetAllQuestionsAsync();
    Result<Product> GetById(Guid id);
    Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProduct dto);
    Task<Result<Product>> DeleteByIdAsync(Guid id);
}
