using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTCTest.Models;
using OTCWebApi.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OTCTest.Controllers
{
    public class UserGridController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        public IActionResult ShowUsers()
        {
            string role = User.FindFirst(x => x.Type == "IsAdmin").Value;
            if (role == "False")
                return View("ShowUsersSimple");

            return View("ShowUsersComplex");
        }

        [HttpGet]
        public IActionResult UserEdit([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery]int userId)
        {
            User user = proxyServiceCallingWeb.GetUser(userId).Result;
            UserEdit userEdit = new UserEdit
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


            return View(userEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserEdit([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, UserEdit user)
        {
            if (ModelState.IsValid)
            {
                User userToSave = new User
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
                var result = proxyServiceCallingWeb.SaveUser(userToSave);

                if (result == HttpStatusCode.OK)
                    return RedirectToAction("ShowUsers", "UserGrid");

                ModelState.AddModelError("Ошибка", "Ошибка сохранения атрибутов пользователя.");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserNew([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, UserEdit user)
        {
            if (ModelState.IsValid)
            {
                User userToSave = new User
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
                var result = proxyServiceCallingWeb.NewUser(userToSave);

                if (result == HttpStatusCode.OK)
                    return RedirectToAction("ShowUsers", "UserGrid");

                ModelState.AddModelError("Ошибка", "Ошибка сохранения атрибутов пользователя.");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult UserNew()
        {
            UserEdit userNew = new UserEdit();

            return View(userNew);
        }
    }
}
