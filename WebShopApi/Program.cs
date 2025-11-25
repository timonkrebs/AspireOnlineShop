using WebShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IProductService, SqlProductService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/product", async (IProductService productService) =>
    {
        var products = await productService.GetProductsAsync();
        return Results.Ok(products);
    })
    .WithName("GetProducts")
    .WithOpenApi();

app.MapGet("/api/product/{id}", async (int id, IProductService productService) =>
    {
        var product = await productService.GetByIdAsync(id);
        return product is null ? Results.NotFound() : Results.Ok(product);
    })
    .WithName("GetProduct")
    .WithOpenApi();

app.Run();