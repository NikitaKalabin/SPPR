using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_Kalabin.UI.Services.CategoryService;

public interface ICategoryService
{
    public Task<ResponseData<List<Category>>> GetGenreListAsync();
}