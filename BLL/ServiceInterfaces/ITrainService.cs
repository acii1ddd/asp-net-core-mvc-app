using BLL.DTO;

namespace BLL.ServiceInterfaces
{
    public interface ITrainService : IService<TrainDTO>
    {
        IEnumerable<TrainDTO> GetTrainsByCity(string city);
    }
}
