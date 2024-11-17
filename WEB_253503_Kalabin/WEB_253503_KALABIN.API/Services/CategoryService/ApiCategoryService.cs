using Microsoft.EntityFrameworkCore;
using WEB_253503_KALABIN.API.Data;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.CategoryService;

namespace WEB_253503_KALABIN.API.Services.CategoryService;

public class ApiCategoryService(AppDbContext dbContext) : ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var result = dbContext.ClothesCategories.ToList();
        return Task.FromResult(ResponseData<List<Category>>.Success(result));
    }

    public Task<ResponseData<Category>> GetCategoryAsync(int id)
    {
        var result = dbContext.ClothesCategories.First(c => c.Id == id);
        return Task.FromResult(ResponseData<Category>.Success(result));
    }

    public async Task UpdateCategoryAsync(int id, Category category)
    {
        dbContext.Attach(category).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
    {
        dbContext.ClothesCategories.Add(category);
        await dbContext.SaveChangesAsync();
        return ResponseData<Category>.Success(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await dbContext.ClothesCategories.FindAsync(id);
        if (category != null)
        {
            dbContext.ClothesCategories.Remove(category);
            await dbContext.SaveChangesAsync();
        }
    }
}