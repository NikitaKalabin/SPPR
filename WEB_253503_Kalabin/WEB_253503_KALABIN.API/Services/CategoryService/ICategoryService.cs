using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;

namespace WEB_253503_KALABIN.API.Services.CategoryService;

public interface ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    
    public Task<ResponseData<Category>> GetCategoryAsync(int id);

    public Task UpdateCategoryAsync(int id, Category category);

    public Task<ResponseData<Category>> CreateCategoryAsync(Category category);

    public Task DeleteCategoryAsync(int id);
}