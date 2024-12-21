using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    public class PassengerProfile:Profile
    {
        public PassengerProfile()
        {
            CreateMap<Passenger, PassengerDTO>().ReverseMap();
        }
    }
}
