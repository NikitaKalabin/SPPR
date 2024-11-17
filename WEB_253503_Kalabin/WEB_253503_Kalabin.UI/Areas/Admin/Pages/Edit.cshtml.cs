using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IClothesService _clothesService;
        private readonly ICategoryService _categoryService;
        
        public EditModel(IClothesService clothesService, ICategoryService categoryService)
        {
            _clothesService = clothesService;
            _categoryService = categoryService;
        }
        
        [BindProperty]
        public Clothes Clothes { get; set; } = default!;
        
        [BindProperty]
        public IFormFile? ImageFile { get; set; } = default!;
        
        [BindProperty]
        public List<Category> Categorys { get; set; } = default!;
        
        [BindProperty]
        public int ChosenCategoryId { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _categoryService.GetCategoryListAsync();

            if (response.Successfull)
            {
                Categorys = response.Data;
            }
            
            var responseClothess = await _clothesService.GetClothesByIdAsync(id);

            if (responseClothess.Successfull)
            {
                Clothes = responseClothess.Data;
                return Page();
            }
            
            return NotFound();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();
            var Categorys = new List<Category>();

            if (response.Successfull)
            {
                Categorys = response.Data;
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Clothes.Category = ChosenCategoryId;
            
            await _clothesService.UpdateClothesAsync(Clothes.Id, Clothes, ImageFile);
        
            return RedirectToPage("./Index");
        }
        
        private async Task<bool> ClothesExists(int id)
        {
            return (await _clothesService.GetClothesByIdAsync(id)).Successfull;
        }
    }
}
