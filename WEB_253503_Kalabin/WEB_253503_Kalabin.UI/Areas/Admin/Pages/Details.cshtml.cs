using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_Kalabin.UI.Services.ClothesService;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IClothesService _ClothesService;

        public DetailsModel(IClothesService ClothesService)
        {
            _ClothesService = ClothesService;
        }

        public Clothes Clothes { get; set; } = default!;

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
    }
}
