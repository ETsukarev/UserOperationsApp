using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserWebApi.Proxy;

namespace UserMVCApp.Controllers
{
    public class HelperController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public JsonResult GetAllUsers([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery] serverSideParams serverSide)
        {
            var task = Task.Run(() => proxyServiceCallingWeb.GetAllUsers(serverSide));
            var users = task.Result;
            var jsonObj = Json(new { users });  
            return jsonObj;
        }

        // GET: /<controller>/GetNoAdmins
        [HttpGet]
        public JsonResult GetNoAdmins([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery] serverSideParams serverSide)
        {
            var task = Task.Run(() => proxyServiceCallingWeb.GetAllUsersWithoutAdmins(serverSide));
            var users = task.Result;
            var jsonObj = Json(new { users });
            return jsonObj;
        }

        [HttpPost]
        public HttpStatusCode Delete([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, int userId)
        {
            var status = proxyServiceCallingWeb.DeleteUser(userId);
            return status;
        }

        // GET: /<controller>/ExistThatLogin
        [HttpGet]
        public JsonResult ExistThatLogin([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, [FromQuery] string login)
        {
            var result = proxyServiceCallingWeb.ExistThatLogin(login);


            if (result.Error == null && !result.ResultCheckLogin)
            {
                return Json(true);
            }

            if (result.ResultCheckLogin)
            {
                return Json("Пользователь с таким логином уже присутствует. Выберите другое имя !");
            }
            else if (result.Error != null)
            {
                return Json(result.Error.Message);
            }

            return Json(true);
        }
    }
}
