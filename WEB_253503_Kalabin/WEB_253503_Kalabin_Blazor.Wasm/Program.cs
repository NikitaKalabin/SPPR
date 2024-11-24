using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253503_Kalabin_Blazor.Wasm;
using WEB_253503_Kalabin_Blazor.Wasm.Models;
using WEB_253503_Kalabin_Blazor.Wasm.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uriData = new UriData
{
    ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(uriData.ApiUri) });

builder.Services.AddHttpClient<IBlazorClothesService, BlazorClothesService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddOidcAuthentication(options =>
{
// Configure your authentication provider options here.
// For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

await builder.Build().RunAsync();