using Microsoft.EntityFrameworkCore;
using WEB_253503_KALABIN.API.Data;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_KALABIN.API.Services.ClothesService;

public class ApiClothesService(AppDbContext dbContext) : IClothesService
{
    
    public Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > 20) pageSize = 20;
        if (pageSize <= 0) pageSize = 3;

        List<Clothes> searchResult;

        if (categoryNormalizedName is null)
        {
            searchResult = dbContext.Clothes.ToList();
        }
        else
        {
            var id = dbContext.ClothesCategories.First(c => c.NormalizedName == categoryNormalizedName).Id;

            searchResult = dbContext.Clothes
                .Where(a => a.Category == id)
                .ToList();
        }


        int totalPages = searchResult.Count % pageSize == 0 ?
            searchResult.Count / pageSize :
            searchResult.Count / pageSize + 1;

        if (pageNo > totalPages - 1)
        {
            return Task.FromResult(
                ResponseData<ListModel<Clothes>>
                    .Error("No such page"));
        }

        return Task.FromResult(
            ResponseData<ListModel<Clothes>>
                .Success(
                    new ListModel<Clothes> { Items = searchResult.Skip(pageSize * pageNo).Take(pageSize).ToList(), CurrentPage = pageNo, TotalPages = totalPages }
                ));
    }

    public Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync()
    {
        return Task.FromResult(ResponseData<ListModel<Clothes>>.Success(new ListModel<Clothes>{ Items = dbContext.Clothes.ToList(), CurrentPage = 0, TotalPages = 1 }));
    }

    public Task<ResponseData<Clothes>> GetClothesByIdAsync(int id)
    {
        var found = dbContext.Clothes.First(a => a.Id == id);
        return Task.FromResult(ResponseData<Clothes>.Success(found));
    }

    public Task UpdateClothesAsync(int id, Clothes product, IFormFile? formFile)
    {
        dbContext.Attach(product).State = EntityState.Modified;	
        return Task.FromResult(dbContext.SaveChanges());
    }

    public Task DeleteClothesAsync(int id)
    {
        var toRemove = dbContext.Clothes.First(a => a.Id == id);
        dbContext.Clothes.Remove(toRemove);
        return Task.FromResult(dbContext.SaveChanges());
    }

    public Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes)
    {
        dbContext.Clothes.Add(clothes);
        dbContext.SaveChanges();
        return Task.FromResult(ResponseData<Clothes>.Success(clothes));
    }

    public async Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes, IFormFile? file)
    {
        dbContext.Clothes.Add(clothes);
        await dbContext.SaveChangesAsync();
        return ResponseData<Clothes>.Success(clothes);
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile? formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            return ResponseData<string>.Error("Invalid image file.");
        }

        var clothes = await dbContext.Clothes.FindAsync(id);
        if (clothes == null)
        {
            return ResponseData<string>.Error("Clothes not found.");
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }

        clothes.Image = "/images/" + uniqueFileName;
        dbContext.Attach(clothes).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return ResponseData<string>.Success(clothes.Image);
    }
}