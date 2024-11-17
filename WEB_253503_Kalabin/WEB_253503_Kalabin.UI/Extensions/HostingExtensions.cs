using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

namespace WEB_253503_Kalabin.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(this WebApplicationBuilder builder)
    {
        // builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
        // builder.Services.AddScoped<IClothesService, MemoryClothesService>();
    }
}