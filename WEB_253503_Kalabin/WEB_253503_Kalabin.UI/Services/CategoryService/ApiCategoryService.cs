using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;

namespace WEB_253503_Kalabin.UI.Services.CategoryService;

public class ApiCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public ApiCategoryService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>($"{_httpClient.BaseAddress!.AbsoluteUri}Category");
            return response ?? ResponseData<List<Category>>.Error("Failed to fetch categories.");
    }

    public async Task<ResponseData<Category>> GetCategoryAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ResponseData<Category>>($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}");
        return response ?? ResponseData<Category>.Error("Failed to fetch category.");
    }

    public async Task UpdateCategoryAsync(int id, Category category)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category", category);
        var createdCategory = await response.Content.ReadFromJsonAsync<ResponseData<Category>>();
        return createdCategory ?? ResponseData<Category>.Error("Failed to create category.");
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}");
        response.EnsureSuccessStatusCode();
    }
}