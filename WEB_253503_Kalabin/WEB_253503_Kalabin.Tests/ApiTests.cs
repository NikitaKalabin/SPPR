using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_253503_KALABIN.API.Data;
using WEB_253503_KALABIN.API.Services.ClothesService;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using Xunit;

namespace WEB_253503_Kalabin.Tests
{
    public class ApiClothesServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly ApiClothesService _clothesService;

        public ApiClothesServiceTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureCreated();

            _clothesService = new ApiClothesService(_dbContext);
        }

        [Fact]
        public async Task GetProductListAsync_ReturnsFirstPageByDefault()
        {
            // Arrange
            SeedClothes(10);

            // Act
            var result = await _clothesService.GetClothesListAsync(null, 1, 3);

            // Assert
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(4, result.Data.TotalPages);
        }

        [Fact]
        public async Task GetProductListAsync_SelectsCorrectPage()
        {
            // Arrange
            SeedClothes(10);

            // Act
            var result = await _clothesService.GetClothesListAsync(null, 2, 3);

            // Assert
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(4, result.Data.TotalPages);
            Assert.Equal("Clothes4", result.Data.Items.First().Name);
        }

        [Fact]
        public async Task GetProductListAsync_FiltersByCategory()
        {
            // Arrange
            SeedClothes(10);
            var category = _dbContext.ClothesCategories.First().Id;

            var categoryName =  await _dbContext.ClothesCategories.FindAsync(category);

            // Act
            var result = await _clothesService.GetClothesListAsync(categoryName.NormalizedName, 1, 3);

            // Assert
            Assert.All(result.Data.Items, p => Assert.Equal(category, p.Category));
        }

        [Fact]
        public async Task GetProductListAsync_DoesNotAllowPageSizeGreaterThanMax()
        {
            // Arrange
            SeedClothes(10);

            // Act
            var result = await _clothesService.GetClothesListAsync(null, 1, 25);

            // Assert
            Assert.Equal(10, result.Data.Items.Count);
        }

        [Fact]
        public async Task GetProductListAsync_ReturnsErrorIfPageNumberExceedsTotalPages()
        {
            // Arrange
            SeedClothes(10);

            // Act
            var result = await _clothesService.GetClothesListAsync(null, 5, 3);

            // Assert
            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }

        private void SeedClothes(int count)
        {
            var category = new Category { Name = "Category1", NormalizedName = "category1" };
            _dbContext.ClothesCategories.Add(category);
            _dbContext.SaveChanges();

            for (int i = 1; i <= count; i++)
            {
                _dbContext.Clothes.Add(new Clothes { Name = $"Clothes{i}", Category = category.Id });
            }
            _dbContext.SaveChanges();
        }
    }
}