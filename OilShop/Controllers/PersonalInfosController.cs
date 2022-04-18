using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OilShop.Models;
using OilShop.ViewModels.PersonalInfos;
using OilShop.ViewModels.Users;
using System.Threading.Tasks;

namespace OilShop.Controllers
{
    [Authorize(Roles = "admin, registeredUser,manager")]

    public class PersonalInfosController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppCtx _context;

        public PersonalInfosController(UserManager<User> userManager,
        AppCtx context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: PersonalInfosController
        public async Task<ActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            IndexPersonalInfosViewModel model = new()
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Patronymic = user.Patronymic
            };

            return View(model);
        }

        // GET: PersonalInfosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexPersonalInfosViewModel model)
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (model.LastName == null || model.FirstName == null || model.Patronymic == null)
            {
                ModelState.AddModelError(string.Empty, "Заполните все поля");
            }

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.LastName = model.LastName;
                    user.FirstName = model.FirstName;
                    user.Patronymic = model.Patronymic;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ChangePassword()
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            IndexPersonalInfosViewModel model = new() { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(IndexPersonalInfosViewModel model)
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (model.Password == null || model.PasswordConfirm == null)
            {
                ModelState.AddModelError(string.Empty, "Введите пароль");
            }

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ChangeEmail()
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            IndexPersonalInfosViewModel model = new() { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(IndexPersonalInfosViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userASP = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                User user = new()
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                // добавляем пользователя
                if (_userManager.GetUserIdAsync(user) == null)
                {
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "Index",
                        "PersonalInfos",
                        new { code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new();
                    await emailService.SendEmailAsync(model.Email, "Confirm E-mail",
                        $"Подтвердите электронную почту, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return View("RegisterConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким E-mail уже существует");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View();
            else
                return View("Error");
        }
    }
}
