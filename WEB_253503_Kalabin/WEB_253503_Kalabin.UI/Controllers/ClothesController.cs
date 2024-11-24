using Microsoft.AspNetCore.Mvc;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Extensions;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Controllers;

public class ClothesController : Controller
{
    private IClothesService _clothesService;
    private ICategoryService _categoryService;

    public ClothesController(IClothesService clothesService, ICategoryService categoryService)
    {
        _clothesService = clothesService;
        _categoryService = categoryService;
    }

    [Route("catalog/{category?}")]
    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        try
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
            {
                return NotFound(categoryResponse.ErrorMessage);
            }

            var clothesResponse = await _clothesService.GetClothesListAsync(category, pageNo);
            if (!clothesResponse.Successfull)
            {
                return NotFound(clothesResponse.ErrorMessage);
            }

            var request = HttpContext.Request;
            string currentCategoryNormalizedName = request.Query["category"].ToString();

            ViewData["categories"] = categoryResponse.Data;

            if (string.IsNullOrEmpty(category))
            {
                ViewData["currentCategory"] = "Все";
            }
            else
            {
                ViewData["currentCategoryNormalizedName"] = currentCategoryNormalizedName;
                if (categoryResponse.Data!.Find(g => g.NormalizedName == category) is null)
                {
                    return NotFound("No category with this name.");
                }

                ViewData["currentCategory"] = categoryResponse.Data!.Find(g => g.NormalizedName == category)!.Name;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Components/Clothes/_ClothesListPartial.cshtml",
                    clothesResponse.Data);
            }

            return View(clothesResponse.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
