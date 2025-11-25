using System.Data;
using WebShopApi.Models;
using Microsoft.Data.SqlClient;

namespace WebShopApi.Services;

public class SqlProductService : IProductService
{
    private readonly string _connectionString;

    public SqlProductService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Sql")
                            ?? throw new InvalidOperationException("ConnectionStrings:Sql not found.");
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var result = new List<Product>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"
SELECT Id, Name, Description, Price, ImageBase64, Stock
FROM Products
ORDER BY Name;";

        using var cmd = new SqlCommand(sql, connection);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                ImageBase64 = reader.IsDBNull(reader.GetOrdinal("ImageBase64"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ImageBase64")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
            };

            result.Add(product);
        }

        return result;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"
SELECT Id, Name, Description, Price, ImageBase64, Stock
FROM Products
WHERE Id = @Id;";

        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        var product = new Product
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                ? string.Empty
                : reader.GetString(reader.GetOrdinal("Description")),
            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
            ImageBase64 = reader.IsDBNull(reader.GetOrdinal("ImageBase64"))
                ? null
                : reader.GetString(reader.GetOrdinal("ImageBase64")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
        };

        return product;
    }
}