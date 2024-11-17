// using Microsoft.AspNetCore.Mvc;
// using WEB_253503_Kalabin.Domain.Entities;
// using WEB_253503_Kalabin.Domain.Models;
// using WEB_253503_Kalabin.UI.Services.CategoryService;
//
// namespace WEB_253503_Kalabin.UI.Services.ClothesService;
//
// public class MemoryClothesService : IClothesService
// {
//     private List<Clothes> _clothes;
//     private readonly List<Category> _categories;
//
//     private IConfiguration _configuration;
//
//     public MemoryClothesService(ICategoryService categoryService, [FromServices] IConfiguration configuration)
//     {
//         _categories = categoryService.GetGenreListAsync().Result.Data;
//         _configuration = configuration;
//
//         SetupData();
//     }
//
//     private void SetupData()
//     {
//         _clothes = new List<Clothes>
//         {
//             new Clothes
//             {
//                 Id = 1,
//                 Name = "Футболка \"Спортивная\"",
//                 Description = "Удобная и стильная футболка для занятий спортом.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("t-shirts")),
//                 Price = 150,
//                 Image = "Images/tshirt.webp",
//             },
//             new Clothes
//             {
//                 Id = 2,
//                 Name = "Джинсы \"Классические\"",
//                 Description = "Прочные и долговечные джинсы для повседневного использования.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("jeans")),
//                 Price = 250,
//                 Image = "Images/jeans.webp",
//             },
//             new Clothes
//             {
//                 Id = 3,
//                 Name = "Куртка \"Зимняя\"",
//                 Description = "Теплая куртка для холодных зимних дней.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("jackets")),
//                 Price = 500,
//                 Image = "Images/jacket.webp",
//             },
//             new Clothes
//             {
//                 Id = 4,
//                 Name = "Платье \"Летнее\"",
//                 Description = "Легкое и воздушное платье для теплых летних дней.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("dresses")),
//                 Price = 300,
//                 Image = "Images/dress.webp",
//             },
//             new Clothes
//             {
//                 Id = 5,
//                 Name = "Обувь \"Кроссовки\"",
//                 Description = "Удобные кроссовки для бега и повседневной носки.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("shoes")),
//                 Price = 200,
//                 Image = "Images/sneakers.webp",
//             },
//             new Clothes
//             {
//                 Id = 6,
//                 Name = "Шорты \"Повседневные\"",
//                 Description = "Удобные шорты для прогулок и отдыха.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("shorts")),
//                 Price = 180,
//                 Image = "Images/shorts.webp",
//             },
//             new Clothes
//             {
//                 Id = 7,
//                 Name = "Рубашка \"Классическая\"",
//                 Description = "Элегантная рубашка для деловых встреч и официальных мероприятий.",
//                 Category = _categories.Find(category => category.NormalizedName.Equals("shirts")),
//                 Price = 220,
//                 Image = "Images/shirt.webp",
//             },
//         };
//
//     }
//     
//     public Task<ResponseData<ListModel<Clothes>>> GetShowListAsync(string? categoryNormalizedName, int pageNo = 1)
//     {
//         var clothes = _clothes.Where(show => categoryNormalizedName == null || show.Category.NormalizedName == categoryNormalizedName);
//         var itemsPerPage = _configuration.GetValue<int>("ItemsPerPage");
//         ListModel<Clothes> showListModel = new()
//         {
//             Items = clothes.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
//             CurrentPage = pageNo,
//             TotalPages = (int)Math.Ceiling((double)clothes.Count() / itemsPerPage)
//         };
//         var result = ResponseData<ListModel<Clothes>>.Success(showListModel);
//         return Task.FromResult(result);
//     }
//     
//     public Task<ResponseData<List<Clothes>>> GetShowListAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ResponseData<Clothes>> GetShowByIdAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task UpdateShowAsync(int id, Clothes clothes, IFormFile? formFile)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteShowAsync(int id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task CreateShowAsync(Clothes clothes, IFormFile? formFile)
//     {
//         throw new NotImplementedException();
//     }
// }