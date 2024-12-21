using BLL.DTO;

namespace BLL.ServiceInterfaces
{
    public interface ITicketService : IService<TicketDTO>
    {
        IEnumerable<TicketDTO> GetTicketsByRoute(string source, string destination);
    }
}
