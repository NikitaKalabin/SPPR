using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_253503_KALABIN.API.Data;
using WEB_253503_KALABIN.API.Services.CategoryService;
using WEB_253503_KALABIN.API.Services.ClothesService;
using WEB_253503_Kalabin.UI.Services.CategoryService;
using WEB_253503_Kalabin.UI.Services.ClothesService;
using WEB_253503_Kalabin.Domain.Models;

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

var authServer = builder.Configuration
    .GetSection("AuthServer")
    .Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.MetadataAddress = $"{authServer!.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
            o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
            o.Audience = "account";
            o.RequireHttpsMetadata = false;
        }
    );

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER_USER"));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//await DbInitializer.SeedData(app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();