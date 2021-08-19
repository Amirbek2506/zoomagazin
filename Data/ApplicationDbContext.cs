using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZooMag.Entities;

namespace ZooMag.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
          // Database.EnsureCreated();
        }

        #region products
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Callback> Callbacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BrandCategory> BrandCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductGalery> ProductGaleries { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Comment> Comments { get; set; }
        #endregion

        public DbSet<ProductItemImage> ProductItemImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PickupPoint> PickupPoints { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }

        #region product orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProductItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        #endregion
       

        #region chat
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Chat> Chats { get; set; }
        #endregion


        #region hotel
        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxType> BoxTypes { get; set; }
        public DbSet<BoxOrder> BoxOrders { get; set; }
        #endregion


        #region pet order
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<PetOrder> PetOrders { get; set; }
        #endregion


        #region additional
        public DbSet<SlideShow> SlideShows { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<SiteProperty> SiteProperties { get; set; }
        public DbSet<Article> Articles { get; set; }
        #endregion

        #region additional service
        public DbSet<AdditionalServ> AdditionalServs{ get; set; }
        public DbSet<ServImages> ServImages { get; set; }
        #endregion

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PetTransport> PetTransports { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProductItem>().HasKey(x => new {x.OrderId, x.ProductItemId});
            builder.Entity<Basket>().HasKey(x => new {x.UserId, x.ProductItemId});
            builder.Entity<Wishlist>().HasKey(x => new {x.UserId, x.ProductItemId});
            builder.Entity<Category>().HasOne(x => x.ParentCategory).WithMany(x=>x.Categories)
                .HasForeignKey(x => x.ParentCategoryId).IsRequired(false);
            builder.Entity<BrandCategory>().HasKey(x => new { x.BrandId, x.CategoryId });
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
                Title = "не выбрано",
            });
            builder.Entity<Gender>().HasData(new Gender
            {
                Id = 2,
                Title = "мужчина",
            });
            builder.Entity<Gender>().HasData(new Gender
            {
                Id = 3,
                Title = "женщина",
            });


            var hasher = new PasswordHasher<IdentityUser<int>>();
            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "admin".ToUpper(),
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


             builder.Entity<BoxType>().HasData(new BoxType
             {
                Id = 1,
                Title = "Для кошек",
             });
             builder.Entity<BoxType>().HasData(new BoxType
             {
                Id = 2,
                Title = "Для собак",
             });
 
            base.OnModelCreating(builder);
        }
    }
}
