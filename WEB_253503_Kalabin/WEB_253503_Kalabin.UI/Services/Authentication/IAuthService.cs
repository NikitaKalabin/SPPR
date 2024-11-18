namespace WEB_253503_Kalabin.UI.Services.Authentication;

public interface IAuthService
{
    Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar);
}