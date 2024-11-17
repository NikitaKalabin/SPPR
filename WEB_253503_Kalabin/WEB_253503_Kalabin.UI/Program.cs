using WEB_253503_Kalabin.UI;
using WEB_253503_Kalabin.UI.Extensions;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;
using WEB_253503_Kalabin.UI.Services.FileService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.RegisterCustomServices();

var uriData = new UriData
{
    ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services
    .AddHttpClient<IFileService, FileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<IClothesService, ClothesService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<ICategoryService, CategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

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