using System.Text;
using System.Net.Http.Headers;
using WEB_253503_Kalabin.UI.Services.Authentication;

namespace WEB_253503_Kalabin.UI.Services.FileService;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenAccessor _tokenAccessor;
    public FileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _tokenAccessor = tokenAccessor;
        //_httpContext = httpContextAccessor.HttpContext;
    }
    public async Task DeleteFileAsync(string fileUri)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}imageupload/{fileUri}").ToString();
        var response = await _httpClient.DeleteAsync(urlString);
    }
    public async Task<string> SaveFileAsync(IFormFile formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}imageupload").ToString();

        var extension = Path.GetExtension(formFile.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        var content = new MultipartFormDataContent();

        var streamContent = new StreamContent(formFile.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

        content.Add(streamContent, "file", newName);

        var response = await _httpClient.PostAsync(urlString, content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        return String.Empty;
    }

}