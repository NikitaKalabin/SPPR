using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;

namespace WEB_253503_Kalabin.UI.Services.ClothesService;

public class ApiClothesService : IClothesService
{
    private readonly HttpClient _httpClient;

    public ApiClothesService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Clothes>>>(
                $"{_httpClient.BaseAddress!.AbsoluteUri}Clothes?category={categoryNormalizedName}&pageNo={pageNo}&pageSize={pageSize}");
            return response ?? ResponseData<ListModel<Clothes>>.Error("Failed to fetch clothes.");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON deserialization error: {ex.Message}");
            return ResponseData<ListModel<Clothes>>.Error("Failed to fetch clothes due to a deserialization error.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return ResponseData<ListModel<Clothes>>.Error("Failed to fetch clothes due to an unexpected error.");
        }
    }

    public async Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Clothes>>>($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes");
        return response ?? ResponseData<ListModel<Clothes>>.Error("Failed to fetch clothes.");
    }

    public async Task<ResponseData<Clothes>> GetClothesByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ResponseData<Clothes>>($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/{id}");
        return response ?? ResponseData<Clothes>.Error("Failed to fetch clothes.");
    }

    public async Task UpdateClothesAsync(int id, Clothes product, IFormFile? formFile)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/{id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteClothesAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes", clothes);
        var createdClothes = await response.Content.ReadFromJsonAsync<ResponseData<Clothes>>();
        return createdClothes ?? ResponseData<Clothes>.Error("Failed to create clothes.");
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile? formFile)
    {
        if (formFile == null)
        {
            return ResponseData<string>.Error("Invalid image file.");
        }

        using var content = new MultipartFormDataContent();
        using var fileStream = formFile.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType);
        content.Add(fileContent, "formFile", formFile.FileName);

        var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress!.AbsoluteUri}clothes/{id}/image", content);
        var imageUrl = await response.Content.ReadFromJsonAsync<ResponseData<string>>();
        return imageUrl ?? ResponseData<string>.Error("Failed to save image.");
    }
}