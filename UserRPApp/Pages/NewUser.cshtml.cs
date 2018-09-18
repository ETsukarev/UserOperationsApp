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

        public IActionResult OnPost([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb)
        {
            if (ModelState.IsValid)
            {
                var strError = proxyServiceCallingWeb.ExistLogin(Person.Login, 0);
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

                    if (userToSave.Password == null)
                        userToSave.Password = string.Empty;

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