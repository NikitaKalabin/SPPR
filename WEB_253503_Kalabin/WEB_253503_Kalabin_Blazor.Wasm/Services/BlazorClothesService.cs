using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WEB_253503_Kalabin.Domain.Entities;
using WEB_253503_Kalabin.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace WEB_253503_Kalabin_Blazor.Wasm.Services;

public class BlazorClothesService : IBlazorClothesService
{
    public event Action? DataLoaded;
    public List<Category> Categories { get; set; }
    public List<Clothes> Clothes { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public Category? SelectedCategory { get; set; }
    
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IAccessTokenProvider _tokenProvider;
    private ILogger<BlazorClothesService> _logger;
    
    public BlazorClothesService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider, 
        ILogger<BlazorClothesService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
    
    public async Task GetClothesListAsync(int pageNo = 1)
    {
        if (_httpClient == null || _configuration == null || _tokenProvider == null)
        {
            throw new InvalidOperationException("Service dependencies are not properly initialized.");
        }

        var tokenRequest = await _tokenProvider.RequestAccessToken();
        if (!tokenRequest.TryGetToken(out var token))
        {
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
        var pageSize = _configuration.GetSection("ItemsPerPage").Value;

        var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}Clothes/categories";
        List<KeyValuePair<string, string>> queryData = new();

        if (SelectedCategory is not null)
        {
            urlString += $"/{SelectedCategory.NormalizedName}";
        }
        else
        {
            urlString += "/all";
        }

        if (pageNo > 1)
        {
            queryData.Add(KeyValuePair.Create("pageNo", pageNo.ToString()));
        }

        if (!pageSize.Equals("3"))
        {
            queryData.Add(KeyValuePair.Create("pageSize", pageSize));
        }

        if (queryData.Count > 0)
        {
            urlString = QueryHelpers.AddQueryString(urlString, queryData);
        }

        var response = await _httpClient.GetAsync(new Uri(urlString));

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Clothes>>>();
            if (data != null)
            {
                if (data.Data != null)
                {
                    Clothes = data.Data.Items;
                    Success = data.Successfull;
                    TotalPages = data.Data.TotalPages;
                    CurrentPage = data.Data.CurrentPage;
                }

                ErrorMessage = data.ErrorMessage ?? "";
            }

            if (Clothes.Count == 0)
            {
                ErrorMessage = "No items available for the selected category.";
            }
        }
        else
        {
            Clothes = new List<Clothes>();
            Success = false;
            TotalPages = 0;
            CurrentPage = 0;
            ErrorMessage = "Failed to load data.";
        }
        DataLoaded?.Invoke();
    }
    public async Task GetCategoryListAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Category");
        
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
                Categories = data!.Data!;
                ErrorMessage = data.ErrorMessage ?? "";
            }
            catch(JsonException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}