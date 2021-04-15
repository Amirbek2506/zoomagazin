using AutoMapper;
using ZooMag.Models;
using ZooMag.Models.Entity;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.Hostel;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.Models.ViewModels.PetTransports;
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
            CreateMap<Order, OutOrderModel>().ReverseMap();
            CreateMap<Product, OutProductModel>().ReverseMap();
            CreateMap<Product, FirstProductModel>().ReverseMap();
            CreateMap<Product, UpdProductModel>().ReverseMap();
            CreateMap<Cart, CartModel>().ReverseMap();
            CreateMap<Size, SizeModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<PetTransport, InpPetTransportModel>().ReverseMap();
            CreateMap<PetTransport, OutPetTransport>().ReverseMap();
            CreateMap<BoxOrder, OutBoxOrderModel>().ReverseMap();
        }
    }
}
