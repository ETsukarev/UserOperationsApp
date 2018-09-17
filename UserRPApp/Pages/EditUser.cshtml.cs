using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiddleWareWebApi;
using MiddleWareWebApi.Models;
using UserRPApp.Models;

namespace UserRPApp.Pages
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public UserEdit Person { get; set; }

        [BindProperty]
        public string password { get; set; }

        public IActionResult OnGetOneUser([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery]int userId)
        {
            User user = proxyServiceCallingWeb.GetUser(userId).Result;
            Person = new UserEdit
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                Telephone = user.Telephone
            };

            password = user.Password;

            return Page();
        }

        public IActionResult OnPost([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb)
        {
            if (ModelState.IsValid)
            {
                var strError = proxyServiceCallingWeb.ExistLogin(Person.Login, Person.Id);
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
                        FirstName = Person.FirstName,
                        MiddleName = Person.MiddleName,
                        LastName = Person.LastName,
                        IsAdmin = Person.IsAdmin,
                        Telephone = Person.Telephone,
                        Password = Person.Password ?? password
                    };

                    var result = proxyServiceCallingWeb.SaveUser(userToSave);

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