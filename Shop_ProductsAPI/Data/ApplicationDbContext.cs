using Microsoft.EntityFrameworkCore;
using Shop_ProductsAPI.Models;

namespace Shop_ProductsAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet <Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "First product",
                    Description = "Description of product",
                    Price = 10,
                    ImageUrl = "",
                    CreatedDate= DateTime.Now
                },
                new Product()
                {
                    Id = 2,
                    Name = "Second product",
                    Description = "Description of product",
                    Price = 15,
                    ImageUrl = "",
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
