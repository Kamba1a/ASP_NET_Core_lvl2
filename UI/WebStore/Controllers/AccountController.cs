using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; //позволяет аутентифицировать пользователя и устанавливать или удалять его куки
        }

        //public IActionResult Login(string returnUrl)
        //{
        //    return View(new LoginUserViewModel() { ReturnUrl = returnUrl});
        //}

        public IActionResult Login(string pathUrl, string queryString)
        {
            //string returnUrl = HttpContext.Request.Headers["Referer"].ToString(); //получает абсолютный адрес, с которого пользователь перешел на страницу логина (вместе с get-запросом)
            
            string returnUrl = pathUrl + queryString; //релятивная ссылка + get-запрос(если есть)
            return View(new LoginUserViewModel() { ReturnUrl = returnUrl}); // передаем модель в представление со ссылкой куда вернуться
        }

        public IActionResult Registration()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //т.к. в представлении есть аналогичный атрибут asp-antiforgery="true"
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("","Ошибка входа");
                return View(model);
            }
            if (Url.IsLocalUrl(model.ReturnUrl)) return Redirect(model.ReturnUrl); //проверка на то, что ReturnUrl является ссылкой с нашего сайта
                                                                                   //кажется, немного бесполезной проверкой, т.к. абсолютные пути через нее проверять смысла нет (все false),
                                                                                   //поэтому выше используются только методы для получения локальных ссылок, что делает эту проверку бесполезной
                                                                                   //(т.е. даже если перейти с другого сайта, ссылка будет вида "/" и все-равно приведет на главную нашего сайта)

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model); //валидация

            User user = new User { UserName = model.UserName, Email = model.Email };
            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password); //создать пользователя

            if (!identityResult.Succeeded) //если не удалось создать пользователя
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    return View(model); //выводит первую ошибку
                }
            }

            await _signInManager.SignInAsync(user, false); //логин пользователя после регистрации (без пароля)
            await _userManager.AddToRoleAsync(user, "Users"); //добавляем зарегистрировавшегося к общей группе пользователей

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken] //метод post т.к. ValidateAntiForgeryToken не работает с get ?
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}