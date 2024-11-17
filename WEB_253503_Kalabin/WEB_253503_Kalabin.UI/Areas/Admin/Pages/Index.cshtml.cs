using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IClothesService _ClothesService;

        public IndexModel(IClothesService ClothesService)
        {
            _ClothesService = ClothesService;
        }

        public IList<Clothes> Clothes { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Clothes = (await _ClothesService.GetClothesListAsync()).Data.Items;
        }
    }
}
