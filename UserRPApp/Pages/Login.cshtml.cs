using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiddleWareWebApi;

namespace UserRPApp.Pages
{
    public class LoginModel : PageModel
    {
        [Required(ErrorMessage = "Укажите имя пользователя !")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Используйте только латинские буквы")]
        [BindProperty]
        public string Login  { get; set; }

        [DataType(DataType.Password)]
        [BindProperty]
        public string Password { get; set; }

        private readonly IProxyServiceCallingWebApi _proxyServiceCallingWebApi;

        public LoginModel(IProxyServiceCallingWebApi proxyServiceCallingWebApi)
        {
            _proxyServiceCallingWebApi = proxyServiceCallingWebApi;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userTask = _proxyServiceCallingWebApi.CheckUserAndPasswd(Login, Password);
                var user = userTask.Result;

                if (!string.IsNullOrEmpty(user.FirstName)) //успешная аутентификация
                {
                    await Authenticate(user.Login, user.IsAdmin); // аутентификация

                    if (user.IsAdmin)
                    {   // если залогинился админ
                        var url = Url.Page("AllUsers");
                        return Redirect(url);
                    }
                    else
                    {   // если залогинился простой пользователь 
                        var url = Url.Page("NoAdminUsers");
                        return Redirect(url);
                    }
                }

                ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
            }
            return Page();
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
    }
}