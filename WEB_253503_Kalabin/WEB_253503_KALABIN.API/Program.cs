using Microsoft.EntityFrameworkCore;
using WEB_253503_KALABIN.API.Data;
using WEB_253503_KALABIN.API.Services.CategoryService;
using WEB_253503_KALABIN.API.Services.ClothesService;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
builder.Services.AddScoped<IClothesService, ApiClothesService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//await DbInitializer.SeedData(app);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();