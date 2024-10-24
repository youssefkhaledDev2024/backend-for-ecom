using AutoMapper;
using TestApp.DAL.Entities;
using TestApp.ModelClasses;

namespace TestApp.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            //CreateMap<Categories , CategoryModel>().ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            CreateMap<CategoryModel, Categories>().ReverseMap();

        }
    }
}
