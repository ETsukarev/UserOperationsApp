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
        public IActionResult GetNoAdmins([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb)
        {
            var users = Task.Run(proxyServiceCallingWeb.GetAllUsersWithoutAdmins).Result;
            var jsonObj = Json(new { data = users });
            return jsonObj;
        }

        [HttpPost]
        public HttpStatusCode Delete([FromServices] IProxyServiceCallingWebApi proxyServiceCallingWeb, int userId)
        {
            var status = proxyServiceCallingWeb.DeleteUser(userId);
            return status;
        }

    }
}
