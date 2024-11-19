using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.Authentication;

namespace WEB_253503_Kalabin.UI.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenAccessor _tokenAccessor;

    public CategoryService(HttpClient httpClient, IConfiguration configuration, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _tokenAccessor = tokenAccessor;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        //await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>($"{_httpClient.BaseAddress!.AbsoluteUri}Category");
            return response ?? ResponseData<List<Category>>.Error("Failed to fetch categories.");
    }

    public async Task<ResponseData<Category>> GetCategoryAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.GetFromJsonAsync<ResponseData<Category>>($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}");
        return response ?? ResponseData<Category>.Error("Failed to fetch category.");
    }

    public async Task UpdateCategoryAsync(int id, Category category)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.PutAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category", category);
        var createdCategory = await response.Content.ReadFromJsonAsync<ResponseData<Category>>();
        return createdCategory ?? ResponseData<Category>.Error("Failed to create category.");
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Category/{id}");
        response.EnsureSuccessStatusCode();
    }
}