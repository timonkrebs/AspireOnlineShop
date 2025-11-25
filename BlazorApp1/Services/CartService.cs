using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class CartService
{
    private readonly List<CartItem> _items = new();


    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();


    public void Add(Product product, int qty = 1)
    {
        if (qty <= 0) return;
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing is null)
        {
            _items.Add(new CartItem { Product = product, Quantity = Math.Min(qty, product.Stock) });
        }
        else
        {
            existing.Quantity = Math.Min(existing.Quantity + qty, product.Stock);
        }
    }


    public void Remove(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item is not null)
        {
            _items.Remove(item);
        }
    }


    public void UpdateQuantity(int productId, int qty)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item is null) return;
        item.Quantity = Math.Clamp(qty, 1, item.Product.Stock);
    }


    public void Clear()
    {
        _items.Clear();
    }


    public decimal Subtotal => _items.Sum(i => i.LineTotal);
    public decimal Shipping => _items.Any() ? 4.90m : 0m;
    public decimal Total => Subtotal + Shipping;
}