using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZooMag.Models;
using ZooMag.Models.Entity;

namespace ZooMag.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<SlideShow> SlideShows { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGalery> ProductGaleries { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SiteProperty> SiteProperties { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<AnimalGender> AnimalGenders { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxOrder> BoxOrders { get; set; }
        public DbSet<BoxType> BoxTypes { get; set; }
        public DbSet<PetOrder> PetOrders { get; set; }
        public DbSet<PetTransport> PetTransports { get; set; }
        public DbSet<Brand> Brands { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Администратор",
                NormalizedName = "Администратор".ToUpper()
            });
            builder.Entity<Role>().HasData(new Role
            {
                Id = 2,
                Name = "Бухгалтер",
                NormalizedName = "Бухгалтер".ToUpper()
            });
            builder.Entity<Role>().HasData(new Role
            {
                Id = 3,
                Name = "Клиент",
                NormalizedName = "Клиент".ToUpper()
            });



            builder.Entity<Gender>().HasData(new Gender
            {
                Id = 1,
                TitleRu = "не выбрано",
                TitleEn = "not chosen"
            });
            builder.Entity<Gender>().HasData(new Gender
            {
                Id = 2,
                TitleRu = "мужчина",
                TitleEn = "man"
            });
            builder.Entity<Gender>().HasData(new Gender
            {
                Id = 3,
                TitleRu = "женщина",
                TitleEn = "woman"
            });


            var hasher = new PasswordHasher<IdentityUser<int>>();
            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "user@example.com",
                NormalizedUserName = "user@example.com".ToUpper(),
                Email = "user@example.com",
                NormalizedEmail = "user@example.com".ToUpper(),
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "string"),
                SecurityStamp = string.Empty,
                GenderId = 1,
                Image = "Resources/Images/Users/useravatar.svg"
            });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = 1
            });


            builder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                Id = 1,
                Title = "Новый заказ"
            });
            builder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                Id = 2,
                Title = "Обработан"
            });
            builder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                Id = 3,
                Title = "Отказ"
            });
            builder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                Id = 4,
                Title = "Доставлен"
            });
            builder.Entity<PaymentMethod>().HasData(new PaymentMethod
            {
                Id = 1,
                MethodName = "Оплата после получение товара"
            });



             builder.Entity<AnimalGender>().HasData(new AnimalGender
             {
                Id = 1,
                TitleRu = "женский",
                TitleEn = "female"
             });
             builder.Entity<AnimalGender>().HasData(new AnimalGender
             {
                Id = 2,
                TitleRu = "мужской",
                TitleEn = "male"
             });
             builder.Entity<AnimalGender>().HasData(new AnimalGender
             {
                Id = 3,
                TitleRu = "гермафродит (такими животные тоже бывают)",
                TitleEn = "hermaphrodite"
             });
 
            
             builder.Entity<AnimalType>().HasData(new AnimalType
             {
                Id = 1,
                TitleRu = "кошка(кот)",
                TitleEn = "cat"
             });
             builder.Entity<AnimalType>().HasData(new AnimalType
             {
                Id = 2,
                TitleRu = "собака(кобель)",
                TitleEn = "dog"
             });
 
            
             builder.Entity<BoxType>().HasData(new BoxType
             {
                Id = 1,
                TitleRu = "Для кошек",
                TitleEn = "For cat"
             });

             builder.Entity<BoxType>().HasData(new BoxType
             {
                Id = 2,
                TitleRu = "Для собак",
                TitleEn = "For dog"
             });
 
            base.OnModelCreating(builder);
        }
    }
}
