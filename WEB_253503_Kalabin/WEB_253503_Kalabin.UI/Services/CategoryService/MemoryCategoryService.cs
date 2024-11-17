// using WEB_253503_Kalabin.Domain.Entities;
// using WEB_253503_Kalabin.Domain.Models;
// using WEB_253503_Kalabin.UI.Services.CategoryService;
//
// namespace WEB_253503_Kalabin.UI.Services.CategoryService;
//
// public class MemoryCategoryService : ICategoryService
// {
//     public Task<ResponseData<List<Category>>> GetGenreListAsync()
//     {
//         var clothingCategoriesList = new List<Category>
//         {
//             new Category
//             {
//                 Id = 1,
//                 Name = "Футболки",
//                 NormalizedName = "t-shirts",
//             },
//             new Category
//             {
//                 Id = 2,
//                 Name = "Джинсы",
//                 NormalizedName = "jeans",
//             },
//             new Category
//             {
//                 Id = 3,
//                 Name = "Куртки",
//                 NormalizedName = "jackets",
//             },
//             new Category
//             {
//                 Id = 4,
//                 Name = "Платья",
//                 NormalizedName = "dresses",
//             },
//             new Category
//             {
//                 Id = 5,
//                 Name = "Обувь",
//                 NormalizedName = "shoes",
//             },
//             new Category
//             {
//                 Id = 6,
//                 Name = "Шорты",
//                 NormalizedName = "shorts",
//             },
//             new Category
//             {
//                 Id = 7,
//                 Name = "Рубашки",
//                 NormalizedName = "shirts",
//             },
//         };
//
//
//         var result = ResponseData<List<Category>>.Success(clothingCategoriesList);
//
//         return Task.FromResult(result);
//     }
// }