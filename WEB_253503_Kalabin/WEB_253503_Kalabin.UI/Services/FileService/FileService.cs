using System.Text;
using System.Net.Http.Headers;

namespace WEB_253503_Kalabin.UI.Services.FileService;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    public FileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        //_httpContext = httpContextAccessor.HttpContext;
    }
    public async Task DeleteFileAsync(string fileUri)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}imageupload/{fileUri}").ToString();
        var response = await _httpClient.DeleteAsync(urlString);
    }
    public async Task<string> SaveFileAsync(IFormFile formFile)
    {
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