using AutoMapper;
using BLL.DTO;
using lab_3.Models;

namespace lab_3.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketDTO, TicketViewModel>().ReverseMap();
        }
    }
}
