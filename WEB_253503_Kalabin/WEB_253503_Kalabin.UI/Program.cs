using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253503_Kalabin.UI;
using WEB_253503_Kalabin.UI.Extensions;
using WEB_253503_Kalabin.UI.HelperClasses;
using WEB_253503_Kalabin.UI.Services.Authentication;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;
using WEB_253503_Kalabin.UI.Services.FileService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.RegisterCustomServices();
builder.Services.AddHttpContextAccessor();

var uriData = new UriData
{
    ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

builder.Services
    .AddHttpClient<IFileService, FileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<IClothesService, ClothesService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<ICategoryService, CategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
            OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer()
    .AddOpenIdConnect(options =>
        {
            options.Authority = $"{keycloakData!.Host}/auth/realms/{keycloakData.Realm}";
            options.ClientId = keycloakData.ClientId;
            options.ClientSecret = keycloakData.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("openid");
            options.SaveTokens = true;
            options.RequireHttpsMetadata = false;
            options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();