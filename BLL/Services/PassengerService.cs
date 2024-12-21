using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;

        // внедрение репозитория из DAL
        public PassengerService(IPassengerRepository repository, IMapper mapper)
        {
            _passengerRepository = repository;
            _mapper = mapper;
        }

        public void Add(PassengerDTO item)
        {
            var passenger = _mapper.Map<Passenger>(item);
            _passengerRepository.Add(passenger);
        }

        public void DeleteById(int id)
        {
            var passenger = _passengerRepository.Get(id);
            if (passenger != null)
            {
                _passengerRepository.Delete(passenger);
            }
        }

        public IEnumerable<PassengerDTO> GetAll()
        {
            var passengers = _passengerRepository.GetAll();
            return _mapper.Map<IEnumerable<PassengerDTO>>(passengers);
        }

        public PassengerDTO? GetById(int id)
        {
            var passenger = _passengerRepository.Get(id);
            return passenger == null ? null : _mapper.Map<PassengerDTO>(passenger);
        }

        public IEnumerable<PassengerDTO> GetPassengersByName(string name)
        {
            var passengers = _passengerRepository.GetAll();

            if (passengers == null || string.IsNullOrWhiteSpace(name))
            {
                return Enumerable.Empty<PassengerDTO>(); // данных нет или имя пустое
            }
            var filtred = passengers.Where(p => p.FirstName.ToLower() == name.ToLower());
            return _mapper.Map<IEnumerable<PassengerDTO>>(filtred);
        }

        public void Update(PassengerDTO item)
        {
            var passenger = _passengerRepository.Get(item.Id); // старый
            if (passenger != null)
            {
                // обновляем passenger из бд, используя новый PassrngerDTO
                _mapper.Map(item, passenger); // phone и passport data обновляться не будут
                _passengerRepository.Update(passenger); // обновляем в репозитории пассажиров
            }
        }
    }
}
