using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly IMapper _mapper;
        private ILogger<TicketService> _logger;

        public TicketService(ITicketRepository ticketRepository, ITrainRepository trainRepository, IMapper mapper, ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _trainRepository = trainRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public void Add(TicketDTO item)
        {
            if (item.Source == item.Destination)
                throw new ArgumentException("Точка отпавления совпадает с точкой прибытия.", nameof(item.StartTime));

            if (item.StartTime < DateTime.Now)
                throw new ArgumentException("Дата отправления не может быть в прошлом.", nameof(item.StartTime));

            if (item.ArrivalTime <= item.StartTime)
                throw new ArgumentException("Время прибытия должно быть больше, чем время отправления.", nameof(item.ArrivalTime));

            if (item.WagonNumber <= 0 && item.WagonNumber > 25)
                throw new ArgumentException("Некорректный номер вагона.", nameof(item.WagonNumber));

            if (item.PlaceNumber <= 0 && item.PlaceNumber > 50)
                throw new ArgumentException("Некорректный номер места.", nameof(item.PlaceNumber));

            var ticket = _mapper.Map<Ticket>(item);
            _ticketRepository.Add(ticket);
        }

        public void DeleteById(int id)
        {
            var ticket = _ticketRepository.Get(id);
            if (ticket != null)
            {
                _ticketRepository.Delete(ticket);
            }
        }

        public IEnumerable<TicketDTO> GetAll()
        {
            var tickets = _ticketRepository.GetAll();
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public TicketDTO? GetById(int id)
        {
            var ticket = _ticketRepository.Get(id);
            return ticket == null ? null : _mapper.Map<TicketDTO>(ticket);
        }

        public IEnumerable<TicketDTO> GetTicketsByRoute(string source, string destination)
        {
            if (source.Equals(destination))
                throw new ArgumentException("Пункт отправления не может совпадать с пунктом назначения.", nameof(source));

            _logger.LogInformation($"Поиск билетов с пунктом отправления '{source}' и пунктом назначения '{destination}'.");

            var tickets = _ticketRepository.GetAll()
                .Where(ticket => ticket.Source.Equals(source, StringComparison.OrdinalIgnoreCase) 
                && ticket.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // если билеты не найдены
            if (!tickets.Any())
            {
                _logger.LogWarning($"Билеты с пунктом отправления '{source}' и пунктом назначения '{destination}' не найдены.");
                return Enumerable.Empty<TicketDTO>();
            }

            _logger.LogInformation($"{tickets.Count()} билетов найдено для маршрута '{source}' -> '{destination}'.");
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public void Update(TicketDTO item)
        {
            if (item.Source.Equals(item.Destination))
                throw new ArgumentException("Пункт отправления не может совпадать с пунктом назначения.", nameof(item.Source));

            if (item.StartTime < DateTime.Now)
                throw new ArgumentException("Дата отправления не может быть в прошлом.", nameof(item.StartTime));

            if (item.ArrivalTime <= item.StartTime)
                throw new ArgumentException("Время прибытия должно быть больше, чем время отправления.", nameof(item.ArrivalTime));

            if (item.WagonNumber <= 0 && item.WagonNumber > 25)
                throw new ArgumentException("Некорректный номер вагона.", nameof(item.WagonNumber));

            if (item.PlaceNumber <= 0 && item.PlaceNumber > 50)
                throw new ArgumentException("Некорректный номер места.", nameof(item.PlaceNumber));

            var ticket = _ticketRepository.Get(item.Id);
            if (ticket != null)
            {
                _mapper.Map(item, ticket); // обновляем ticket с помощью item (Dto)
                _ticketRepository.Update(ticket); // обновляем в репозитории
            }
        }
    }
}
