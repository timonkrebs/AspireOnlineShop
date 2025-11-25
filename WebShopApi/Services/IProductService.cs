using WebShopApi.Models;

namespace WebShopApi.Services;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product?> GetByIdAsync(int id);
}