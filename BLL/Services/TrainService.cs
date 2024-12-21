using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class TrainService : ITrainService
    {
        private readonly ITrainRepository _trainRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainService> _logger;

        public TrainService(ITrainRepository trainRepository, IMapper mapper, ILogger<TrainService> logger)
        {
            _trainRepository = trainRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public void Add(TrainDTO item)
        {
            if (item.Capacity > 200 || item.Capacity < 50)
                throw new ArgumentException("Количество мест не может быть больше 200 и меньше 50.", nameof(item.Capacity));

            if (item.WagonCount <= 5 || item.WagonCount > 30)
                throw new ArgumentException("Количество вагонов не может быть больше 20 и меньше 5.", nameof(item.WagonCount));

            var ticket = _mapper.Map<Train>(item);
            _trainRepository.Add(ticket);
        }

        public void DeleteById(int id)
        {
            var train = _trainRepository.Get(id);
            if (train != null)
            {
                _trainRepository.Delete(train);
            }
        }

        public IEnumerable<TrainDTO> GetAll()
        {
            var trains = _trainRepository.GetAll();
            return _mapper.Map<IEnumerable<TrainDTO>>(trains);
        }

        public TrainDTO? GetById(int id)
        {
            var train = _trainRepository.Get(id);
            return train == null ? null : _mapper.Map<TrainDTO>(train); // null если нету
        }

        public IEnumerable<TrainDTO> GetTrainsByCity(string city)
        {
            _logger.LogInformation($"Поиск поездов с маршрутами с городом '{city}'.");

            var trains = _trainRepository.GetAll()
                .Where(train => train.Route.Contains(city, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // если билеты не найдены
            if (!trains.Any())
            {
                _logger.LogWarning($"Поезда с маршрутом с поездом'{city}' не найдены.");
                return Enumerable.Empty<TrainDTO>();
            }

            _logger.LogInformation($"Найдено {trains.Count()} поездов для маршрута с городом'{city}'.");
            return _mapper.Map<IEnumerable<TrainDTO>>(trains);
        }

        public void Update(TrainDTO item)
        {
            if (item.Capacity > 200 || item.Capacity < 50)
                throw new ArgumentException("Количество мест не может быть больше 200 и меньше 50.", nameof(item.Capacity));

            if (item.WagonCount <= 5 || item.WagonCount > 30)
                throw new ArgumentException("Количество вагонов не может быть больше 30 и меньше 5.", nameof(item.WagonCount));

            var train = _trainRepository.Get(item.Id);
            if (train != null)
            {
                _mapper.Map(item, train);
                _trainRepository.Update(train);
            }
        }
    }
}
