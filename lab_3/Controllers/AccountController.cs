using lab_3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace lab_3.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, 
                    isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {loginModel.UserName} вошел успешно.");
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Ваш аккаунт заблокирован.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильное имя пользователя или пароль.");
                }

                // опять форма со входом
                return View();
            }
            catch
            {
                return View();
            }
        }
            
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(int id)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(registerModel);
                }

                var user = new User 
                {    
                    UserName = registerModel.UserName,
                    FirstName = registerModel.Firstname,
                    LastName = registerModel.Lastname,
                    PhoneNumber = registerModel.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, registerModel.Password); // сделали пользователя

                if (result.Succeeded)
                {
                    // даем роль пассажира
                    var roleResult = await _userManager.AddToRoleAsync(user, "Passenger");

                    if (roleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else if (true)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Произошла ошибка: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize] доступ только авторизированным пользователям (иначе в Login)
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync(); // выход из учетной записи
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["ErrorMessage"] = "Вы не авторизованы!";
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
