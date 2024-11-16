using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.UI.Services.ClothesService;

public interface IClothesService
{
    public Task<ResponseData<ListModel<Clothes>>> GetShowListAsync(string? categoryNormalizedName, int pageNo=1);

    public Task<ResponseData<Clothes>> GetShowByIdAsync();

    public Task UpdateShowAsync(int id, Clothes clothes, IFormFile? formFile);

    public Task DeleteShowAsync(int id);

    public Task CreateShowAsync(Clothes clothes, IFormFile? formFile);
}