using AutoMapper;
using lab_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab_3.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger; 
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, IMapper mapper, ILogger<UserController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _roleManager = roleManager;
        }

        // GET: UserController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = _mapper.Map<UserViewModel>(user); // id тоже мапится
                userViewModel.Role = string.Join(", ", roles.FirstOrDefault()); 
                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            throw new NotImplementedException();
        }

        // GET: UserController/Create
        public async Task <ActionResult> Create()
        {
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewData["Roles"] = allRoles;
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create(UserViewModelToCreate userViewModelToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Добавляемый пользователь не прошел валидацию.");
                    var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    ViewData["Roles"] = allRoles;
                    return View(userViewModelToCreate);
                }

                var user = new User
                {
                    UserName = userViewModelToCreate.UserName,
                    FirstName = userViewModelToCreate.FirstName,
                    LastName = userViewModelToCreate.LastName,
                    PhoneNumber = userViewModelToCreate.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, userViewModelToCreate.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {user.UserName} успешно добавлен.");

                    // добавляем пользователю роль
                    var roleResult = await _userManager.AddToRoleAsync(user, userViewModelToCreate.Role);
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogError($"Ошибка при добавлении роли: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(userViewModelToCreate);
                    }

                    // Редирект на список пользователей
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError($"Ошибка при добавлении пользователя: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                _logger.LogInformation("Билет успешно добавлен. Редирект на Index");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException ex) // ошибка валидации на сервисе
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                // прокидываем ошибки с bll (ошибка будет глобальной, тк key - string.Empty)
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(userViewModelToCreate); // if errors
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            _logger.LogInformation("Метод Edit() для изменения пользователя начал работу.");
            var user = await _userManager.FindByIdAsync(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userViewModel.Role = string.Join(", ", roles.FirstOrDefault());

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewData["Roles"] = allRoles;

            _logger.LogInformation("Пользователь отредактирован успешно.");
            return View(userViewModel);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserViewModel userViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Изменяемый пользователь не прошел валидацию.");
                    var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    ViewData["Roles"] = allRoles;
                    return View(userViewModel);
                }

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogError($"Пользователь с ID {id} не найден.");
                    throw new Exception("Пользователь с ID {id} не найден.");
                }

                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.PhoneNumber = userViewModel.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    _logger.LogError("Ошибка при обновлении пользователя.");

                    var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    ViewData["Roles"] = allRoles;

                    return View(userViewModel);
                }

                // обновляем роль пользователя
                var currentRoles = await _userManager.GetRolesAsync(user); // все роли пользователя
                var selectedRole = userViewModel.Role; // выбранная роль
                // если содержит - менять не надо - false, иначе true и меняем роль
                if (!currentRoles.Contains(selectedRole))
                {
                    // удаляем роль у юзера
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    // назначаем новую роль
                    await _userManager.AddToRoleAsync(user, selectedRole);
                }

                _logger.LogInformation("Билет успешно изменен.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(userViewModel);
        }

        // POST: UserController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Пользователь удален успешно");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError("Ошибка при удалении пользователя");
                return View();
            }
            catch
            {
                _logger.LogError("Ошибка при удалении пользователя");
                return View();
            }
        }
    }
}
