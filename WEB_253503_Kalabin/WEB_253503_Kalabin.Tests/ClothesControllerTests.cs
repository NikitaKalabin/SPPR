using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Controllers;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;
using Xunit;

namespace WEB_253503_Kalabin.Tests
{
    public class ClothesControllerTests
    {
        private readonly IClothesService _clothesService;
        private readonly ICategoryService _categoryService;
        private readonly ClothesController _controller;

        public ClothesControllerTests()
        {
            _clothesService = Substitute.For<IClothesService>();
            _categoryService = Substitute.For<ICategoryService>();
            _controller = new ClothesController(_clothesService, _categoryService);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "POWER-USER")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_Returns404_WhenCategoryListUnsuccessful()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Error("Error")));

            // Act
            var result = await _controller.Index(null, 1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_Returns404_WhenClothesListUnsuccessful()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(new List<Category>())));
            _clothesService.GetClothesListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Clothes>>.Error("Error")));

            // Act
            var result = await _controller.Index(null, 1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_SetsViewData_WithCategoryList()
        {
            // Arrange
            var categories = new List<Category> { new Category { Name = "Category1", NormalizedName = "category1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
            _clothesService.GetClothesListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Clothes>>.Success(new ListModel<Clothes>())));

            // Act
            var result = await _controller.Index(null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(categories, viewResult.ViewData["categories"]);
        }

        [Fact]
        public async Task Index_SetsViewData_WithCurrentCategory()
        {
            // Arrange
            var categories = new List<Category> { new Category { Name = "Category1", NormalizedName = "category1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
            _clothesService.GetClothesListAsync("category1", 1).Returns(Task.FromResult(ResponseData<ListModel<Clothes>>.Success(new ListModel<Clothes>())));

            // Act
            var result = await _controller.Index("category1", 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Category1", viewResult.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_SetsViewData_WithAllCategory_WhenCategoryIsNull()
        {
            // Arrange
            var categories = new List<Category> { new Category { Name = "Category1", NormalizedName = "category1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
            _clothesService.GetClothesListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Clothes>>.Success(new ListModel<Clothes>())));

            // Act
            var result = await _controller.Index(null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Все", viewResult.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_PassesModel_ToView()
        {
            // Arrange
            var clothesList = new ListModel<Clothes> { Items = new List<Clothes> { new Clothes { Name = "Clothes1" } } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(new List<Category>())));
            _clothesService.GetClothesListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Clothes>>.Success(clothesList)));

            // Act
            var result = await _controller.Index(null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(clothesList, viewResult.Model);
        }
    }
}