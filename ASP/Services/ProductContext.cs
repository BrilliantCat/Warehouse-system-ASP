using Microsoft.EntityFrameworkCore;
using ASP.Models;
namespace ASP.Services
{
    public class ProductContext:DbContext
    {
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<ItemOperation> ItemOperations { get; set; } = null!;
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasIndex(u => u.Name) // Выбираем поле
                .IsUnique();           // Делаем его уникальным
        }
    }
}