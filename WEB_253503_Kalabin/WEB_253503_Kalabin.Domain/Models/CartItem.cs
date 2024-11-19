using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.Domain.Models;

public class CartItem
{
    public Clothes Clothes { get; set; }
    public int Count { get; set; }
}