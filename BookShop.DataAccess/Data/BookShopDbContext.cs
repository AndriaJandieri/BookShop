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
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Product Seed
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Description = "A novel written by American author F. Scott Fitzgerald.",
                    ISBN = "9780743273565",
                    Author = "F. Scott Fitzgerald",
                    ListPrice = 15.99m,
                    Price = 12.99m,
                    Price50 = 10.99m,
                    Price100 = 8.99m
                },
                 new Product
                 {
                     Id = 1,
                     Title = "The Great Gatsby",
                     Description = "A novel written by American author F. Scott Fitzgerald.",
                     ISBN = "9780743273565",
                     Author = "F. Scott Fitzgerald",
                     ListPrice = 15.99m,
                     Price = 12.99m,
                     Price50 = 10.99m,
                     Price100 = 8.99m
                 },
                  new Product
                  {
                      Id = 1,
                      Title = "The Great Gatsby",
                      Description = "A novel written by American author F. Scott Fitzgerald.",
                      ISBN = "9780743273565",
                      Author = "F. Scott Fitzgerald",
                      ListPrice = 15.99m,
                      Price = 12.99m,
                      Price50 = 10.99m,
                      Price100 = 8.99m
                  }

                );
            #endregion

            #region Category Seed
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
            #endregion
        }
    }
}
