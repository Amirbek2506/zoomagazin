using AutoMapper;
using ZooMag.Models;
using ZooMag.ViewModels;

namespace ZooMag.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<ProductImagesModel, ProductGalery>().ReverseMap();
            CreateMap<Size, SizeModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
