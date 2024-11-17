using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.FileService;

namespace WEB_253503_Kalabin.UI.Services.ClothesService;

public class ClothesService : IClothesService
{
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;

    public ClothesService(HttpClient httpClient, IConfiguration configuration,IFileService fileService)
    {
        _httpClient = httpClient;
        _fileService = fileService;
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
        if (product.Image != null && formFile != null)
        {
            await _fileService.DeleteFileAsync(product.Image.Split("/").Last());
            product.Image = null;
        }
        if (formFile != null)
        {
            var url = await SaveImageAsync(id, formFile!);
            product.Image = url.Data;
        }

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

    public async Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes, IFormFile? file)
    {
        if (file != null)
        {
            var url = await SaveImageAsync(0, file!);
            clothes.Image = url.Data;
        }

        //await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "clothes");
        var response = await _httpClient.PostAsJsonAsync(uri, clothes);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Clothes>>();
            return data!;
        }
        //_logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
        return ResponseData<Clothes>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        //await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var url = await _fileService.SaveFileAsync(formFile);
        return ResponseData<string>.Success(url);
    }
}