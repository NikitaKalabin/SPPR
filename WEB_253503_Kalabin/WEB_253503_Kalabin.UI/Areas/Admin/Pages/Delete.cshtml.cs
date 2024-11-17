using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IClothesService _ClothesService;

        public DeleteModel(IClothesService ClothesService)
        {
            _ClothesService = ClothesService;
        }

        [BindProperty]
        public Clothes Clothes { get; set; } = default!;
        
        [BindProperty]
        public IFormFile ImageFile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _ClothesService.GetClothesByIdAsync((int)id);

            if (response.Successfull)
            {
                Clothes = response.Data;
                
                return Page();
            }
            
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            await _ClothesService.DeleteClothesAsync((int)id);
            return RedirectToPage("./Index");
        }
    }
}
