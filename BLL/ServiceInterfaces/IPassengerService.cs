using BLL.DTO;

namespace BLL.ServiceInterfaces
{
    public interface IPassengerService : IService<PassengerDTO>
    {
        // специфичкск cruds
        IEnumerable<PassengerDTO> GetPassengersByName(string name);
    }
}
