using API.Models;
using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> productRepository;
    private readonly IWebHostEnvironment webHostEnvironment;

    public ProductService(IWebHostEnvironment webHostEnvironment, IGenericRepository<Product> productRepository)
    {
        this.webHostEnvironment = webHostEnvironment;
        this.productRepository = productRepository;
    }

    public async Task<Result<Product>> AddProductAsync(CreateProduct dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new("Product name is not valid");

        if (string.IsNullOrWhiteSpace(dto.Description))
            return new("Product name is not valid");

        if (dto.Price <= 0)
            return new("Invalid price");

        if (dto.Image == null)
            return new("Image can not be null");

        var imageUrl = UploadImage(dto.Image);
        if(imageUrl.StartsWith("Error"))
            return new("Image is not valid, retry with another image");

        var entity = new Product()
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            ImageUrl = imageUrl
        };

        try
        {
            var createdEntity = await productRepository.AddAsync(entity);
            return new(true) { Data = createdEntity};
        }
        catch (Exception e)
        {
            Log.Error($"Error in adding new product \nException: {e} \nInnerException: {e.InnerException}" );
            throw new("Couldn't create new product. Contact support.", e);
        }
    }

    public async Task<Result<List<Product>>> GetAllQuestionsAsync()
    {
        try
        {
            var products = await productRepository.GetAll().ToListAsync();

            return new (true) { Data = products };
        }
        catch (Exception e)
        {
            Log.Error($"Error in getting all products \nException: {e} \nInnerException: {e.InnerException}");
            throw new("Couldn't get all products, contact support.", e);
        }
    }

    public Result<Product> GetById(Guid id)
    {
        try
        {
            var existingProduct = productRepository.GetById(id);

            if (existingProduct is null)
                return new("Product with given Id Not Found.");

            return new(true) { Data = existingProduct };
        }
        catch (Exception e)
        {
            Log.Error($"Error in getting a product with given id: {id} \nException: {e} \nInnerException: {e.InnerException}");
            throw new($"Couldn't get product by id: {id}, contact support.", e);
        }
    }

    public async Task<Result<Product>> UpdateProductAsync(Guid id, UpdateProduct dto)
    {
        var product = productRepository.GetById(id);
        if (product == null)    
            return new("Product not found");

        try
        {
            if(dto.Name != null) 
                product.Name = dto.Name;

            if(dto.Description != null) 
                product.Description = dto.Description;

            if (dto.Price != null) 
                product.Price = dto.Price.Value;

            if (dto.Quantity != null)
                product.Quantity = dto.Quantity.Value;

            if (dto.Image != null)
                product.ImageUrl = UploadImage(dto.Image);

            return new(true) { Data = await productRepository.UpdateAsync(product) };
        }
        catch (Exception e)
        {
            Log.Error($"Error in updating a product with given id: {id} \nException: {e} \nInnerException: {e.InnerException}");
            throw new($"Couldn't update the product with given id: {id}, contact support.", e);
        }
    }

    public async Task<Result<Product>> DeleteByIdAsync(Guid id)
    {
        try
        {
            var existingProduct = productRepository.GetById(id);

            if (existingProduct == null)
                return new("Couldn't find the product with give Id ");

            var removedProduct = await productRepository.RemoveAsync(existingProduct);

            return new(true) { Data = removedProduct };
        }
        catch (Exception e)
        {
            Log.Error($"Error in deleting a product with given id: {id} \nException: {e} \nInnerException: {e.InnerException}");
            throw new($"Couldn't delete the product with given id: {id}, contact support.", e);
        }
    }


    private string UploadImage(IFormFile file)
    {
        string contentType = file.ContentType;
        string fileName = Guid.NewGuid() + "_" + file.FileName;

        if (contentType != "image/jpeg" && contentType != "image/png")
            return "Error: Image type is not valid";

        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory)?.FullName;
        string imagesFolder = Path.Combine(projectDirectory, "Images");

        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        string filePath = Path.Combine(imagesFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        return filePath;
    }
} 
