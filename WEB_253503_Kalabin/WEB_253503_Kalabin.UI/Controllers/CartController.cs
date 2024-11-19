using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IClothesService _clothesService;
    private Cart _cart;
    public CartController(IClothesService clothesService, Cart cart)
    {
        _clothesService = clothesService;
        _cart = cart;
    }
    
    [Route("[controller]/add/{id:int}")]
    public async Task<ActionResult> Add(int id, string returnUrl)
    {
        var data = await _clothesService.GetClothesByIdAsync(id);
        var a = 0;
        if (data.Successfull)
        {
            _cart.AddToCart(data.Data);
        }
        return Redirect(returnUrl);
    }

    [Route("[controller]/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        _cart.RemoveItems(id);
        return RedirectToAction("Index");
    }
    
    public IActionResult Index()
    {
        return View(_cart);
    }
}