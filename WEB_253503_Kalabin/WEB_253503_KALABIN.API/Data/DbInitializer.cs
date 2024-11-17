using Microsoft.EntityFrameworkCore;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_KALABIN.API.Data;

public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        var pathPrefix = app.Configuration.GetValue<string>("ImageSource")!;

        await SeedCategories(dbContext);
        await SeedClothes(dbContext, pathPrefix);

        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedCategories(AppDbContext dbContext)
    {
        if (!dbContext.ClothesCategories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Футболки", NormalizedName = "t-shirts" },
                new Category { Id = 2, Name = "Джинсы", NormalizedName = "jeans" },
                new Category { Id = 3, Name = "Куртки", NormalizedName = "jackets" },
                new Category { Id = 4, Name = "Платья", NormalizedName = "dresses" },
                new Category { Id = 5, Name = "Обувь", NormalizedName = "shoes" },
                new Category { Id = 6, Name = "Шорты", NormalizedName = "shorts" },
                new Category { Id = 7, Name = "Рубашки", NormalizedName = "shirts" },
            };

            await dbContext.ClothesCategories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedClothes(AppDbContext dbContext, string pathPrefix)
    {
        if (!dbContext.Clothes.Any())
        {
            var categories = dbContext.ClothesCategories.ToList();

            var clothes = new List<Clothes>
            {
                new Clothes { Id = 1, Name = "Футболка \"Спортивная\"", Description = "Удобная и стильная футболка для занятий спортом.", Category = categories.First(c => c.NormalizedName == "t-shirts").Id, Price = 150, Image = pathPrefix + "Images/tshirt.webp" },
                new Clothes { Id = 2, Name = "Джинсы \"Классические\"", Description = "Прочные и долговечные джинсы для повседневного использования.", Category = categories.First(c => c.NormalizedName == "jeans").Id, Price = 250, Image = pathPrefix + "Images/jeans.webp" },
                new Clothes { Id = 3, Name = "Куртка \"Зимняя\"", Description = "Теплая куртка для холодных зимних дней.", Category = categories.First(c => c.NormalizedName == "jackets").Id, Price = 500, Image = pathPrefix + "Images/jacket.webp" },
                new Clothes { Id = 4, Name = "Платье \"Летнее\"", Description = "Легкое и воздушное платье для теплых летних дней.", Category = categories.First(c => c.NormalizedName == "dresses").Id, Price = 300, Image = pathPrefix + "Images/dress.webp" },
                new Clothes { Id = 5, Name = "Обувь \"Кроссовки\"", Description = "Удобные кроссовки для бега и повседневной носки.", Category = categories.First(c => c.NormalizedName == "shoes").Id, Price = 200, Image = pathPrefix + "Images/sneakers.webp" },
                new Clothes { Id = 6, Name = "Шорты \"Повседневные\"", Description = "Удобные шорты для прогулок и отдыха.", Category = categories.First(c => c.NormalizedName == "shorts").Id, Price = 180, Image = pathPrefix + "Images/shorts.webp" },
                new Clothes { Id = 7, Name = "Рубашка \"Классическая\"", Description = "Элегантная рубашка для деловых встреч и официальных мероприятий.", Category = categories.First(c => c.NormalizedName == "shirts").Id, Price = 220, Image = pathPrefix + "Images/shirt.webp" },
            };

            await dbContext.Clothes.AddRangeAsync(clothes);
            await dbContext.SaveChangesAsync();
        }
    }
}