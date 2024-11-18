using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_KALABIN.API.Services.ClothesService;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_KALABIN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClothesController : ControllerBase
    {
        private readonly IClothesService _clothesService;

        public ClothesController(IClothesService clothesService)
        {
            _clothesService = clothesService;
        }

        // GET: api/Clothes
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Clothes>>>> GetClothes()
        {
            var result = await _clothesService.GetClothesListAsync();
            return result;
        }

        // GET: api/Clothes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Clothes>>> GetClothes(int id)
        {
            var result = await _clothesService.GetClothesByIdAsync(id);
            if (result.Data == null)
            {
                return NotFound();
            }
            return result;
        }

        // PUT: api/Clothes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "POWER_USER")]
        public async Task<IActionResult> PutClothes(int id, Clothes clothes)
        {
            if (id != clothes.Id)
            {
                return BadRequest();
            }

            await _clothesService.UpdateClothesAsync(id, clothes, null);
            return NoContent();
        }
        
        [HttpGet("categories/{category}")]
        public async Task<ActionResult<ResponseData<ListModel<Clothes>>>> GetClothes(string? category, int pageNo = 0, int pageSize = 3)
        {
            if (category == "all") {
                category = null;
            }
            return (await _clothesService.GetClothesListAsync(category, pageNo, pageSize));
        }

        // POST: api/Clothes
        [HttpPost]
        [Authorize(Roles = "POWER_USER")]
        public async Task<ActionResult<Clothes>> PostClothes(Clothes clothes)
        {
            var result = await _clothesService.CreateClothesAsync(clothes);
            return CreatedAtAction("GetClothes", new { id = result.Data.Id }, result.Data);
        }

        // DELETE: api/Clothes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "POWER_USER")]
        public async Task<IActionResult> DeleteClothes(int id)
        {
            await _clothesService.DeleteClothesAsync(id);
            return NoContent();
        }
    }
}