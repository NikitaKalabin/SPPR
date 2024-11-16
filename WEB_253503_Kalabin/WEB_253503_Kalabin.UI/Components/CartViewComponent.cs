using Microsoft.AspNetCore.Mvc;
namespace WEB_253503_Kalabin.UI.Components;

public class Cart : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cartInfo = new
        {
            TotalPrice = 0.0,
            ItemsCount = 0
        };

        return View(cartInfo);
    }
}


// public class CartSummary : ViewComponent
// {
//     private IRepository repository;

//     public CartSummary(IRepository repo)
//     {
//         repository = repo;
//     }

//     public IViewComponentResult Invoke()
//     {
//         // Возвращаем данные для представления компонента
//         var cartData = repository.GetAll();
//         return View(cartData);
//     }
// }

