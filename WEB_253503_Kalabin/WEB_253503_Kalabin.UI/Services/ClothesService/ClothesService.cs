using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using WEB_253503_Kalabin.UI.Services.Authentication;
using WEB_253503_Kalabin.UI.Services.FileService;

namespace WEB_253503_Kalabin.UI.Services.ClothesService;

public class ClothesService : IClothesService
{
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;
    private readonly ITokenAccessor _tokenAccessor;
    private readonly IConfiguration _configuration;
    public ClothesService(HttpClient httpClient, IConfiguration configuration,IFileService fileService, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _fileService = fileService;
        _tokenAccessor = tokenAccessor;
        _configuration = configuration;
    }
    
    public async Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/categories/");
        if (categoryNormalizedName != null) urlString.Append($"{categoryNormalizedName}/");
        else urlString.Append($"all");

        if (pageNo > 0)
        {
            urlString.Append(QueryString.Create("pageno", pageNo.ToString()));
            urlString.Append('&');
        }

        urlString.Append(QueryString.Create("pagesize", pageSize.ToString()));

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                //_logger.LogInformation($"-----> Info: {response.Content.ToJson()}");
                return (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Clothes>>>())!;
            }
            catch (JsonException ex)
            {
                //_logger.LogError($"-----> Ошибка: {ex.Message}");
                return ResponseData<ListModel<Clothes>>.Error($"Ошибка: {ex.Message}");
            }
        }
        //_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        return ResponseData<ListModel<Clothes>>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
    }

    public async Task<ResponseData<ListModel<Clothes>>> GetClothesListAsync()
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Clothes>>>($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes");
        return response ?? ResponseData<ListModel<Clothes>>.Error("Failed to fetch clothes.");
    }

    public async Task<ResponseData<Clothes>> GetClothesByIdAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
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
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.PutAsJsonAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/{id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteClothesAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ResponseData<Clothes>> CreateClothesAsync(Clothes clothes)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
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
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "clothes");
        var response = await _httpClient.PostAsJsonAsync(uri, clothes);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Clothes>>();
            return data!;
        }
        return ResponseData<Clothes>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var url = await _fileService.SaveFileAsync(formFile);
        return ResponseData<string>.Success(url);
    }
}