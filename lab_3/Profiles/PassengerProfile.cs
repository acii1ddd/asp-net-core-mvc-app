using AutoMapper;
using BLL.DTO;
using lab_3.Models;

namespace lab_3.Profiles
{
    public class PassengerProfile : Profile
    {
        public PassengerProfile()
        {
            CreateMap<PassengerDTO, PassengerViewModel>().ReverseMap();
        }
    }
}
