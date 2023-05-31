using API.Models;
using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
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

    public async Task<Result<Product>> AddProduct(CreateProduct dto)
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

    private string UploadImage(IFormFile file)
    {
        string contentType = file.ContentType;
        string fileName = file.FileName;

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
