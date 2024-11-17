using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IClothesService _clothesService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IClothesService clothesService, ICategoryService categoryService)
        {
            _clothesService = clothesService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = await _categoryService.GetCategoryListAsync();

            if (response.Successfull)
            {
                Categories = response.Data;
            }
            
            return Page();
        }
        
        [BindProperty]
        public List<Category> Categories { get; set; } = default!;

        [BindProperty]
        public Clothes Clothes { get; set; } = default!;
        
        [BindProperty]
        public IFormFile? ImageFile { get; set; }
        
        [BindProperty]
        public int ChosenCategoryId { get; set; } = default!;
        
        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();
            var Categories = new List<Category>();

            if (response.Successfull)
            {
                Categories = response.Data;
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Clothes.Category = ChosenCategoryId;

            await _clothesService.CreateClothesAsync(Clothes, ImageFile);
    
            return RedirectToPage("./Index");
        }
    }
}
