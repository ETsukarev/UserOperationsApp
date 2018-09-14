using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiddleWareWebApi;
using MiddleWareWebApi.Models;
using UserRPApp.Models;

namespace UserRPApp.Pages
{
    public class NewUserModel : PageModel
    {
        [BindProperty]
        public UserEdit Person { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        private string ExistThatLogin(IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery] string login)
        {
            var result = proxyServiceCallingWeb.ExistThatLogin(login);


            if (result.Error == null && !result.ResultCheckLogin)
            {
                return string.Empty;
            }

            if (result.ResultCheckLogin)
            {
                return "Пользователь с таким логином уже cуществует. Выберите другое имя !";
            }
            else if (result.Error != null)
            {
                return result.Error.Message;
            }

            return string.Empty;
        }

        public IActionResult OnPost([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb)
        {
            if (ModelState.IsValid)
            {
                var strError = ExistThatLogin(proxyServiceCallingWeb, Person.Login);
                if (!string.IsNullOrEmpty(strError))
                {
                    ModelState.AddModelError("Person.Login", strError);
                }
                else
                {
                    User userToSave = new User
                    {
                        Id = Person.Id,
                        Login = Person.Login,
                        Password = Person.Password,
                        FirstName = Person.FirstName,
                        MiddleName = Person.MiddleName,
                        LastName = Person.LastName,
                        IsAdmin = Person.IsAdmin,
                        Telephone = Person.Telephone
                    };

                    var result = proxyServiceCallingWeb.NewUser(userToSave);

                    if (result == HttpStatusCode.OK)
                    {
                        var url = Url.Page("AllUsers");
                        return Redirect(url);
                    }
                }
            }
            return Page();
        }
    }
}