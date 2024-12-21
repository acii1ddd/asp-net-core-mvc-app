using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using lab_3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab_3.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainController> _logger;

        public TrainController(ITrainService trainService, IMapper mapper, ILogger<TrainController> logger)
        {
            _trainService = trainService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: TrainController
        [Authorize(Roles = "Manager, Passenger")]
        public ActionResult Index(IEnumerable<TrainViewModel> trainsToView = null)
        {
            _logger.LogInformation("Начало обработки запроса к методу Index() для Trains.");

            // если равен null - GetAll(), не равен - ticketsToView
            var trainViewModels = trainsToView ?? _mapper.Map<IEnumerable<TrainViewModel>>(_trainService.GetAll());
            _logger.LogInformation("Метод Index() для Trains успешно отработал.");
            return View(trainViewModels);
        }

        // GET: TrainController/Details/5
        [Authorize(Roles = "Manager, Passenger")]
        public ActionResult SearchMany(string city)
        {
            try
            {
                _logger.LogInformation("Начало обработки запроса к методу SearchMany. Город: " + city);
                var trains = _trainService.GetTrainsByCity(city);
                var trainViewModels = _mapper.Map<IEnumerable<TrainViewModel>>(trains);
                _logger.LogInformation("Метод SearchMany отработал успешно. Количесто найденных: " + trainViewModels.Count());

                // отображаем найденные билеты (отдаем целую страницу)
                return View("Index", trainViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в методе SearchMany: {ex.Message}");
                // При редиректе данные, сохранённые в ModelState, теряются
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
                throw;
            }
        }

        // GET: TrainController/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            _logger.LogInformation("Начало обработки запроса к методу Create() для добавления поезда.");
            return View();
        }

        // POST: TrainController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainViewModel trainViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Добавляемый поезд не прошел валидацию.");
                    return View(trainViewModel);
                }
                _logger.LogInformation("Обработка post запроса с формы для добавления билета.");
                var train = _mapper.Map<TrainDTO>(trainViewModel);
                _trainService.Add(train);
                _logger.LogInformation("Поезд успешно добавлен. Редирект на Index");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex) // ошибка валидации на сервисе
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                // прокидываем ошибки с bll (ошибка будет глобальной, тк key - string.Empty)
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(trainViewModel); // if errors
        }

        // GET: TrainController/Edit/5
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            _logger.LogInformation("Метод Edit() для изменения позда начал работу.");
            var train = _trainService.GetById(id);
            _logger.LogInformation("Метод Edit() отработал успешно. Форма для редактировани отдана.");
            return View(_mapper.Map<TrainViewModel>(train));
        }

        // POST: TrainController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrainViewModel trainViewModel, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid) // валидация модели
                {
                    _logger.LogError("Изменяемый поезд не прошел валидацию.");
                    return View(trainViewModel);
                }
                _trainService.Update(_mapper.Map<TrainDTO>(trainViewModel));
                _logger.LogInformation("Поезд успешно изменен.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex) // ошибка валидации на сервисе
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                // прокидываем ошибки с bll (ошибка будет глобальной, тк key - string.Empty)
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(trainViewModel);
        }

        // GET: TrainController/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Метод Delete для удаления поезда начал работу.");
                _trainService.DeleteById(id);
                _logger.LogInformation("Метод Delete отработал успешно. Билет с id" + id + "удален");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
