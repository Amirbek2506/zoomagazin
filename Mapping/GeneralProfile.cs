using System.Linq;
using AutoMapper;
using ZooMag.DTOs.AdditionalServ;
using ZooMag.DTOs.Pet;
using ZooMag.DTOs.PetImage;
using ZooMag.Entities;
using ZooMag.Models.ViewModels.Brands;
using ZooMag.Models.ViewModels.Carts;
using ZooMag.Models.ViewModels.Categories;
using ZooMag.Models.ViewModels.Hostel;
using ZooMag.Models.ViewModels.Orders;
using ZooMag.Models.ViewModels.PetCategories;
using ZooMag.Models.ViewModels.PetTransports;
using ZooMag.Models.ViewModels.ProductItems;
using ZooMag.Models.ViewModels.Products;
using ZooMag.ViewModels;
using zoomagazin.DTOs.Pet;

namespace ZooMag.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //CreateMap<Entities.Brand, CreateBrandRequest>().ReverseMap();
            //CreateMap<Entities.Brand, BrandResponse>().ReverseMap();
            CreateMap<ProductItem, ProductItemModel>().ReverseMap();
            CreateMap<ProductImagesModel, ProductGalery>().ReverseMap();
            CreateMap<PetImagesModel, PetImage>().ReverseMap();
            CreateMap<Entities.Category, InpCategoryModel>().ReverseMap();
            CreateMap<PetCategory, InpPetCategoryModel>().ReverseMap();
            CreateMap<Brand, InpBrandModel>().ReverseMap();
            CreateMap<Product, InpProductModel>().ReverseMap();
            CreateMap<Order, OutOrderModel>().ReverseMap();
            CreateMap<Product, OutProductModel>().ReverseMap();
            CreateMap<Product, FirstProductModel>().ReverseMap();
            CreateMap<Entities.Product, UpdProductModel>().ReverseMap();
            CreateMap<Pet, OutPetModel>().ReverseMap();
            CreateMap<Pet, InpPetModel>().ReverseMap();
            CreateMap<Cart, CartModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<PetTransport, InpPetTransportModel>().ReverseMap();
            CreateMap<PetTransport, OutPetTransport>().ReverseMap();
            CreateMap<BoxOrder, OutBoxOrderModel>().ReverseMap();
            
            CreateMap<CreatePetRequest, Pet>()
                .ForMember(x => x.MainImageId, option => option.Ignore())
                .ForMember(x => x.PetImages, option => option.Ignore())
                .ForMember(x => x.PetCategory, option => option.Ignore());
            CreateMap<UpdatePetRequest, Pet>()
                .ForMember(x => x.PetImages, option => option.Ignore())
                .ForMember(x => x.PetCategory, option => option.Ignore());
            CreateMap<PetImage, GetPetImageResponse>()
                .ForMember(x => x.Image, option => option.MapFrom(x => x.ImageUrl));
            CreateMap<Pet, GetPetResponse>();
            CreateMap<Pet, PetListItemResponse>()
                .ForMember(x => x.Image, option => option.MapFrom(x =>  (x.MainImageId != null? 
                                                                            x.PetImages.FirstOrDefault(i => i.Id == x.MainImageId).ImageUrl:
                                                                            null)));
            CreateMap<CreateAdditionalServRequest, AdditionalServ>()
                .ForMember(x => x.ServImages, option => option.Ignore());
            CreateMap<AdditionalServ, GetAdditionalServResponse>()
                .ForMember(x => x.ServImages, option => option.MapFrom(x => x.ServImages));
            CreateMap<UpdateAdditionalServRequest, AdditionalServ>()
                .ForMember(x => x.ServImages, option => option.Ignore());
            CreateMap<GetServImageResponse, ServImages>();     
            CreateMap<CreateServImageRequest, ServImages>();     
            CreateMap<ServImages, GetServImageResponse>();                
        }
    }
}
