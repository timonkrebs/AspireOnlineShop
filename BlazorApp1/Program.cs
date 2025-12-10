using BlazorApp1.Components;
using BlazorApp1.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var cfg = builder.Configuration.GetSection("ConnectionStrings")["Redis"];
    var options = ConfigurationOptions.Parse(cfg);
    // Best practice adjustments
    options.ClientName = "RedisDemoApi";
    options.KeepAlive = 30;
    options.ReconnectRetryPolicy = new ExponentialRetry(5000);
    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddHttpClient<IProductService, BackendProductService>(client =>
{
    var uri = builder.Configuration.GetSection("BackendApi")["BaseUrl"];
    client.BaseAddress = new Uri(uri);
});
builder.Services.AddSingleton<CartService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();