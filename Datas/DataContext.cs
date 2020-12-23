using HomeShopping.Models;
using HomeShopping.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeShopping.Datas
{
    public class DataContext :IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(
                new Category()
                {
                    CategoryID = 1,
                    CategoryName = "Iphone"
                },
                new Category()
                {
                    CategoryID = 2,
                    CategoryName = "SamSung"
                });
            builder.Entity<Product>().HasData(
                new Product()
                {
                    ProductId = 1,
                    ProductName = "Iphone X ",
                    ProductImage = "iphoneX.png",
                    Descriptions = "iphoneX 64GB - 256GB",
                    ProductQuantity = 200,
                    ProductPrice = 1000.00,
                    CreateDate = DateTime.Now,
                    ManHinh = "IPS LCD, 6.67, Full HD+",
                    HeDieuHanh = "Android",
                    CPU = "Snapdragon 720G 8 nhân",
                    RAM = "8GB",
                    DungLuongPin = "5020mAh,có sạc nhanh",
                    BoNhoTrong = "128GB",
                    CategoryID = 1
                },
                new Product()
                {
                    ProductId = 2,
                    ProductName = "Samsung Galaxy S10+",
                    ProductImage = "iphoneX.png",
                    Descriptions = "iphoneX 64GB - 256GB",
                    ProductQuantity = 100,
                    ProductPrice = 1000.00,
                    CreateDate = DateTime.Now,
                    ManHinh = "IPS LCD, 6.67, Full HD+",
                    HeDieuHanh = "Android",
                    CPU = "Snapdragon 720G 8 nhân",
                    RAM = "8GB",
                    DungLuongPin = "5020mAh,có sạc nhanh",
                    BoNhoTrong = "128GB",
                    CategoryID = 1
                },
                new Product()
                {
                    ProductId = 3,
                    ProductName = "Sony Xperia Z1",
                    ProductImage = "iphoneX.png",
                    Descriptions = "iphoneX 64GB - 256GB",
                    ProductQuantity = 100,
                    ProductPrice = 1000.00,
                    CreateDate = DateTime.Now,
                    ManHinh = "IPS LCD, 6.67, Full HD+",
                    HeDieuHanh = "Android",
                    CPU = "Snapdragon 720G 8 nhân",
                    RAM = "8GB",
                    DungLuongPin = "5020mAh,có sạc nhanh",
                    BoNhoTrong = "128GB",
                    CategoryID = 1
                },
                new Product()
                {
                    ProductId = 4,
                    ProductName = "Sony Xperia Z5",
                    ProductImage = "iphoneX.png",
                    Descriptions = "iphoneX 64GB - 256GB",
                    ProductQuantity = 100,
                    ProductPrice = 1000.00,
                    CreateDate = DateTime.Now,
                    ManHinh = "IPS LCD, 6.67, Full HD+",
                    HeDieuHanh = "Android",
                    CPU = "Snapdragon 720G 8 nhân",
                    RAM = "8GB",
                    DungLuongPin = "5020mAh,có sạc nhanh",
                    BoNhoTrong = "128GB",
                    CategoryID = 1
                });
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                },
                 new IdentityRole
                 {
                     Name = "Administrator",
                     NormalizedName = "ADMINISTRATOR"
                 }
                );
        }

    }
   
}

