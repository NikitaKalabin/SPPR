using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.UI.Services.ClothesService;

public interface IClothesService
{
    public Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync(string?
        categoryNormalizedName, int pageNo = 1, int pageSize = 3);
    public Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync();
    public Task<ResponseData<Clothes>> GetClothesByIdAsync(int id);
    public Task UpdateClothesAsync(int id, Clothes product, IFormFile? formFile);
    public Task DeleteClothesAsync(int id);
    public Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes);
    public Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes, IFormFile? file);
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile? formFile);
}