using AutoMapper;
using TestApp.DAL.Entities;
using TestApp.ModelClasses;

namespace TestApp.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Products , ProductsModel>().ReverseMap();
        }
    }
}
