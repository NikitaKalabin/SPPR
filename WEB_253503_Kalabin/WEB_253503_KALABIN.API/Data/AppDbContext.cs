using Microsoft.EntityFrameworkCore;
using WEB_253503_Kalabin.Domain.Entities;

namespace WEB_253503_KALABIN.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> contextOptions) : DbContext(contextOptions)
{
    public DbSet<Clothes> Clothes { get; set; }
    public DbSet<Category> ClothesCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}