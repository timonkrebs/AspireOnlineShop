using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class BackendProductService : IProductService
{
    private readonly HttpClient _httpClient;
    
    public BackendProductService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Product>>("api/product") ?? new List<Product>();
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return _httpClient.GetFromJsonAsync<Product?>($"api/product/{id}");
    }
}