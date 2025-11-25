using BlazorApp1.Models;

namespace BlazorApp1.Services;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product?> GetByIdAsync(int id);
}