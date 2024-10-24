using AutoMapper;
using TestApp.DAL.Entities;
using TestApp.ModelClasses;

namespace TestApp.Mapper
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingModel>().ReverseMap();

        }
    }
}
