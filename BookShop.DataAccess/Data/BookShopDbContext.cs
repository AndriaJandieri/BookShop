using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataAccess.Data
{
    public class BookShopDbContext : DbContext
    {
        public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Fiction",
                    Description = "Fictional books"
                },
                new Category
                {
                    Id = 2,
                    Name = "Science",
                    Description = "Scientific books"
                },
                new Category
                {
                    Id = 3,
                    Name = "History",
                    Description = "Historical books"
                });
        }
    }
}
