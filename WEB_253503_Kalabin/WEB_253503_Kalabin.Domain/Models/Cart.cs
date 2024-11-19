using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.Domain.Models;

public class Cart
{
    public List<CartItem> CartItems { get; set; }
    public int TotalPrice
    {
        get => CartItems.Sum(item => item.Clothes.Price * item.Count);
    }

    public int Count
    {
        get => CartItems.Sum(item => item.Count);
    }

    public Cart()
    {
        CartItems = new List<CartItem>();
    }

    public virtual void AddToCart(Clothes clothes)
    {
        var cartItem = CartItems.FirstOrDefault(item => item.Clothes.Id == clothes.Id);
        if (cartItem is null)
        {
            CartItems.Add(new CartItem { Clothes = clothes, Count = 1 });
        }
        else
        {
            cartItem.Count++;
        }
    }
    
    public virtual void RemoveItems(int id)
    {
        var cartItem = CartItems.FirstOrDefault(item => item.Clothes.Id == id);
        if (cartItem is not null)
        {
            if (cartItem.Count > 1)
            {
                cartItem.Count--;
            }
            else
            {
                CartItems.Remove(cartItem);
            }
        }
    }
    
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }
}