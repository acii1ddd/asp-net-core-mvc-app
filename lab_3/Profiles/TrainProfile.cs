using AutoMapper;
using BLL.DTO;
using lab_3.Models;

namespace lab_3.Profiles
{
    public class TrainProfile : Profile
    {
        public TrainProfile()
        {
            CreateMap<TrainDTO, TrainViewModel>().ReverseMap();
        }
    }
}
