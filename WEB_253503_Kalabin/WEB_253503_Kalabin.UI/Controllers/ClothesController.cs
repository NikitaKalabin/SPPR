using Microsoft.AspNetCore.Mvc;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Controllers;

public class ClothesController: Controller
{
    private IClothesService _clothesService;
    private ICategoryService _categoryService;
    
    public ClothesController(IClothesService clothesService, ICategoryService categoryService)
    {
        _clothesService = clothesService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var genreResponse = await _categoryService.GetCategoryListAsync();
        var clothesResponse = await _clothesService.GetClothesListAsync(category, pageNo);

        if (!clothesResponse.Successfull) return NotFound(clothesResponse.ErrorMessage);
        if (!genreResponse.Successfull) return NotFound(genreResponse.ErrorMessage);
        
        var request = HttpContext.Request;
        string? currentGenreNormalizedName = request.Query["category"].ToString();

        ViewData["categories"] = genreResponse.Data;

        if (string.IsNullOrEmpty(category))
        {
            ViewData["currentCategory"] = "Все";
        }
        else
        {
            ViewData["currentCategoryNormalizedName"] = currentGenreNormalizedName;
            ViewData["currentCategory"] = genreResponse.Data!.Find(g => g.NormalizedName == category).Name;
        }
        
        return View(clothesResponse.Data);
    }
}