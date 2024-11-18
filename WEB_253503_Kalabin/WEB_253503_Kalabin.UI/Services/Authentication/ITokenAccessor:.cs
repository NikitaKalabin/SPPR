namespace WEB_253503_Kalabin.UI.Services.Authentication;

public interface ITokenAccessor
{
    Task<string> GetAccessTokenAsync();
    Task SetAuthorizationHeaderAsync(HttpClient httpClient);
}