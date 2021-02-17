﻿using AutoMapper;
using ZooMag.Models;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.Products;
using ZooMag.ViewModels;

namespace ZooMag.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<ProductImagesModel, ProductGalery>().ReverseMap();
            CreateMap<Category, InpCategoryModel>().ReverseMap();
            CreateMap<Product, InpProductModel>().ReverseMap();
            CreateMap<Product, OutProductModel>().ReverseMap();
            CreateMap<Product, UpdProductModel>().ReverseMap();
            CreateMap<Size, SizeModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
