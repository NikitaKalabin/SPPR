namespace WEB_253503_Kalabin.UI.Services.FileService;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile formFile);
    Task DeleteFileAsync(string fileName);
}