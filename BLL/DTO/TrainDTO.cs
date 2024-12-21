using BLL.ServiceInterfaces;

namespace BLL.DTO
{
    public class TrainDTO : IDTO
    {
        public int Id { get; set; }

        public string Route { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public int WagonCount { get; set; }
    }
}
