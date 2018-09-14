using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiddleWareWebApi;
using MiddleWareWebApi.Proxy;

namespace UserRPApp.Pages
{
    public class NoAdminUsersModel : PageModel
    {
        private readonly IProxyServiceCallingWebApi _proxyServiceCallingWebApi;

        public NoAdminUsersModel(IProxyServiceCallingWebApi proxyServiceCallingWebApi)
        {
            _proxyServiceCallingWebApi = proxyServiceCallingWebApi;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnGetNoAdmin([FromQuery] serverSideParams serverSide)
        {
            var task = Task.Run(() => _proxyServiceCallingWebApi.GetAllUsersWithoutAdmins(serverSide));
            var users = task.Result;
            var jsonObj = new JsonResult(new { users });
            return jsonObj;
        }
    }
}