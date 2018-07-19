using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UserMVCApp.Models;

namespace UserMVCApp.Controllers
{
    public class LoginController : Controller
    {
        private IProxyServiceCallingWebApi _proxyServiceCallingWebApi;

        public const string ErrorString = "Ошибка";

        public LoginController(IProxyServiceCallingWebApi proxyServiceCallingWebApi)
        {
            _proxyServiceCallingWebApi = proxyServiceCallingWebApi;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Login()
        {
            var status = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Status;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userTask = _proxyServiceCallingWebApi.CheckUserAndPasswd(model.Login, model.Password);
                var user = userTask.Result;

                if (!user.FirstName.IsNullOrEmpty()) //успешная аутентификация
                {
                    await Authenticate(user.Login, user.IsAdmin); // аутентификация

                    return RedirectToAction("ShowUsers", "UserGrid");
                }

                ModelState.AddModelError(ErrorString, "Некорректные логин и(или) пароль");
            }
            return View(model);
        }


        private async Task Authenticate(string userName, bool isAdmin)
        {
            // создаем клаймы для логина и признака администратора
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("IsAdmin", isAdmin.ToString())
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, "IsAdmin");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var status = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Status;
            return RedirectToAction("Login", "Login");
        }
    }
}
