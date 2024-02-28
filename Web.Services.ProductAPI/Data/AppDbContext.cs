using Microsoft.EntityFrameworkCore;
using Web.Services.ProductAPI.Models;

namespace Web.Services.ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = "Sofa",
                Image = "/images/anh1.jpg",
                Status = "Active"
            }) ;
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 2,
                Name = "ArmChair",
                Image = "/images/anh1.jpg",
                Status = "Active"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 3,
                Name = "WingChair",
                Image = "/images/anh1.jpg",
                Status = "Active"
            });



            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Title = "Sofa1",
                ProductCode = "SF1",
                Description = "Description1 about sofa 1",
                Image = "/images/anh1.jpg",
                Price = 100000,
                CategoryId = 1,
                Status = "Active"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Title = "ArmChair1",
                ProductCode = "AC1",
                Description = "Description1 about ArmChair1",
                Image = "/images/anh11.jpg",
                Price = 200000,
                CategoryId = 2,
                Status = "Active"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Title = "WingChair1",
                ProductCode = "WC1",
                Description = "Description1 about WingChair",
                Image = "/images/anh111.jpg",
                Price = 300000,
                CategoryId = 3,
                Status = "Active"
            });
        }
    }
}
