using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiddleWareWebApi;
using MiddleWareWebApi.Proxy;


namespace UserRPApp.Pages
{
    public class AllUsersModel : PageModel
    {
        private readonly IProxyServiceCallingWebApi _proxyServiceCallingWebApi;

        public AllUsersModel(IProxyServiceCallingWebApi proxyServiceCallingWebApi)
        {
            _proxyServiceCallingWebApi = proxyServiceCallingWebApi;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnGetEveryBody([FromQuery] serverSideParams serverSide)
        {
            var task = Task.Run(() => _proxyServiceCallingWebApi.GetAllUsers(serverSide));
            var users = task.Result;
            var jsonObj = new JsonResult(new { users });
            return jsonObj;
        }

        public IActionResult OnGetDelUser([FromQuery] int userId)
        {
            var status = _proxyServiceCallingWebApi.DeleteUser(userId);
            return StatusCode((int)status);
        }
    }
}