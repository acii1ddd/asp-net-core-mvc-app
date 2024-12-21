using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using lab_3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab_3.Controllers
{
    public class PassengerController : Controller
    {
        private readonly IPassengerService _passengerService;
        private readonly IMapper _mapper;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(IPassengerService passengerService, IMapper mapper, ILogger<PassengerController> logger)
        {
            _passengerService = passengerService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: PassengerController
        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            _logger.LogInformation("Начало обработки запроса к методу Index.");
            var passengers = _passengerService.GetAll();
            var passengerViewModels = _mapper.Map<IEnumerable<PassengerViewModel>>(passengers);
            _logger.LogInformation("Метод Index успешно отработал");
            return View(passengerViewModels);
        }

        // GET: PassengerController/SearchByName?name={name}
        [Authorize(Roles = "Manager")]
        public ActionResult SearchByName(string name)
        {
            _logger.LogInformation("Начало обработки запроса к методу SearchByName. Параметр name: " + name);
            var passengers = _passengerService.GetPassengersByName(name);
            var passengerViewModels = _mapper.Map<IEnumerable<PassengerViewModel>>(passengers);
            _logger.LogInformation("Метод SearchByName отработал успешно. Количесто найденных: " + passengerViewModels.Count());
            return PartialView("_PassengerTablePartial", passengerViewModels); // не включая Layout, только html таблицы
        }

        // GET: PassengerController/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            _logger.LogInformation("Начало обработки запроса к методу Create.");
            return View(); // возвращает форму, которая отправляет post запрос
        }

        // POST: PassengerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PassengerViewModel passengerViewModel)
        {
            try
            {
                if (!ModelState.IsValid) // валидация модели
                {
                    _logger.LogError("Добавляемый пассажир не прошел валидацию.");
                    return View(passengerViewModel);
                }
                _logger.LogInformation("Обработка post запроса с формы для добавления пассажира.");
                var passenger = _mapper.Map<PassengerDTO>(passengerViewModel);
                _passengerService.Add(passenger);
                _logger.LogInformation("Пассажир успешно добавлен. Редирект на Index");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(passengerViewModel); // в случае ошибки возвращаем форму с текущими данными
            }
        }

        // GET: PassengerController/Edit/5
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            _logger.LogInformation("Метод Edit() начал работу.");
            var passenger =_passengerService.GetById(id);
            _logger.LogInformation("Метод Edit() отработал успешно.");
            return View(_mapper.Map<PassengerViewModel>(passenger));
        }

        // POST: PassengerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PassengerViewModel passengerViewModel, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid) // валидация модели
                {
                    _logger.LogError("Изменяемый пассажир не прошел валидацию.");
                    return View(passengerViewModel);
                }
                _passengerService.Update(_mapper.Map<PassengerDTO>(passengerViewModel));
                _logger.LogInformation($"Пассажир {passengerViewModel.FirstName} успешно изменен.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(passengerViewModel);
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Метод Delete начал работу.");
                _passengerService.DeleteById(id);
                _logger.LogInformation($"Метод Delete отработал успешно. Пассажир с id {id} удален");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
