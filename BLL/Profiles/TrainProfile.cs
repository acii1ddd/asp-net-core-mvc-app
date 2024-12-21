using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    public class TrainProfile:Profile
    {
        public TrainProfile()
        {
            CreateMap<Train, TrainDTO>().ReverseMap();
        }
    }
}
