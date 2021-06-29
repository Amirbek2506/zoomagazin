using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using ZooMag.Data;
using ZooMag.Entities;
using ZooMag.Mapping;
using ZooMag.Models;
using ZooMag.Services;
using ZooMag.Services.Interfaces;

namespace ZooMag
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.  
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddCors();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IProductItemService, ProductItemService>();
            services.AddTransient<IDescriptionService, DescriptionService>();
            services.AddTransient<ICallbackService, CallbackService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddHostedService<RemoveOldPromotionsWorker>();
            /*
              services.AddTransient<ICartsService, CartsService>();
              services.AddTransient<IMeasuresService, MeasuresService>();
              services.AddTransient<IWishlistService, WishlistService>();
              services.AddTransient<IOrdersService, OrdersService>();
              services.AddTransient<IAnimalsService, AnimalsService>();
              services.AddTransient<IChatsService, ChatsService>();
              services.AddTransient<IArticlesService, ArticlesService>();
              services.AddTransient<IPetTransportsService, PetTransportsService>();
              services.AddTransient<IPetOrdersService, PetOrdersService>();
              services.AddTransient<IHostelService, HostelService>();
              services.AddTransient<IPetsService, PetsService>();
              services.AddTransient<IPetCategoriesService, PetCategoriesService>();
              services.AddTransient<ISlideShowsService, SlideShowsService>();*/
            services.AddTransient<IBrandsService, BrandsService>();
            services.AddTransient<IFileService, FileService>();

            services.AddControllers();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.AllowedUserNameCharacters += "абвгдеёжзийклмнопрстуфхцшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦШЩЬЫЪЭЮЯ ";
            });

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("ConnStr")));

            services.AddAutoMapper(typeof(Startup), typeof(GeneralProfile));
            // For Identity  
            services.AddIdentity<User, Role>()  
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ZooMagazin",
                    Description = "Nado bistro zaconchit"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://zoomag.tj",
                                        "http://www.zoomag.tj",
                                        "http://localhost:3000")
                    // builder.AllowAnyOrigin()
                                        .AllowAnyHeader().AllowCredentials()
                                        .AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Зоомагазин v1"));
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("MyAllowSpecificOrigins");
            // app.UseCors(o => o.AllowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
