using AutoMapper;
using BLL.DTO;
using BLL.ServiceInterfaces;
using lab_3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab_3.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, IMapper mapper, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: TicketController
        [Authorize(Roles = "Passenger,Manager")]
        public ActionResult Index(IEnumerable<TicketViewModel> ticketsToView = null)
        {
            _logger.LogInformation("Начало обработки запроса к методу Index() для Tickets.");

            // если равен null - GetAll(), не равен - ticketsToView
            var ticketViewModels = ticketsToView ?? _mapper.Map<IEnumerable<TicketViewModel>>(_ticketService.GetAll());
            _logger.LogInformation("Метод Index() для Tickets успешно отработал.");
            return View(ticketViewModels);
        }

        // GET: TicketController/Details/5
        [Authorize(Roles = "Passenger,Manager")]
        public ActionResult SearchMany(string source, string destination)
        {
            try
            {
                _logger.LogInformation("Начало обработки запроса к методу SearchMany. Пункт отправления: " + source + "Пункт назначения" + destination);
                var tickets = _ticketService.GetTicketsByRoute(source, destination);
                var ticketViewModels = _mapper.Map<IEnumerable<TicketViewModel>>(tickets);
                _logger.LogInformation("Метод SearchMany отработал успешно. Количесто найденных: " + ticketViewModels.Count());

                // отображаем найденные билеты (отдаем целую страницу)
                //return View("Index", ticketViewModels);
                return PartialView("_TicketTablePartial", ticketViewModels); // не включая Layout, только html таблицы
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

        // GET: TicketController/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            _logger.LogInformation("Начало обработки запроса к методу Create() для добавления билета.");
            return View();
        }

        // POST: TicketController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketViewModel ticketViewModel)
        {
            try
            {
                if (!ModelState.IsValid) // валидация объекта ticketViewModel
                {
                    _logger.LogError("Добавляемый билет не прошел валидацию.");
                    return View(ticketViewModel);
                }
                _logger.LogInformation("Обработка post запроса с формы для добавления билета.");
                var ticket = _mapper.Map<TicketDTO>(ticketViewModel);
                _ticketService.Add(ticket);
                _logger.LogInformation("Билет успешно добавлен. Редирект на Index");
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
            return View(ticketViewModel); // if errors
        }

        // GET: TicketController/Edit/5
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            _logger.LogInformation("Метод Edit() для изменения билета начал работу.");
            var ticket = _ticketService.GetById(id);
            _logger.LogInformation("Метод Edit() отработал успешно. Форма для редактирования отдана.");
            return View(_mapper.Map<TicketViewModel>(ticket));
        }

        // POST: TicketController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TicketViewModel ticketViewModel, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid) // валидация модели
                {
                    _logger.LogError("Изменяемый билет не прошел валидацию.");
                    return View(ticketViewModel);
                }
                _ticketService.Update(_mapper.Map<TicketDTO>(ticketViewModel));
                _logger.LogInformation("Билет успешно изменен.");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex) // ошибка валидации на сервисе
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(ticketViewModel);
        }

        // POST: TicketController/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Метод Delete для удаления билета начал работу.");
                _ticketService.DeleteById(id);
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
