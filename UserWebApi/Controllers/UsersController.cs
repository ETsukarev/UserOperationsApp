using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserWebApi.Models;
using UserWebApi.Proxy;

namespace UserWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        readonly UserContext _dbContext;

        public UsersController(UserContext context)
        {
            _dbContext = context;
        }

        // GET: api/<controller>/ExistThatLogin?
        [HttpGet("ExistThatLogin")]
        public bool ExistThatLogin([FromQuery]string loginCheck)
        {
            var result = _dbContext.Users.FirstOrDefault(usr => usr.Login.Equals(loginCheck));
            return result != null;
        }

        // GET: api/<controller>/AllUsers?
        [HttpGet("AllUsers")]
        public ServerSidePage Get([FromQuery]serverSideParams serverSidePrms)
        {
            var users = from user in _dbContext.Users select user;
            var recsTotal = users.Count();

            if (!string.IsNullOrEmpty(serverSidePrms.searchValue))
            {
                users = users.Where(s => s.Login.Contains(serverSidePrms.searchValue)
                                      || s.FirstName.Contains(serverSidePrms.searchValue)
                                      || s.MiddleName.Contains(serverSidePrms.searchValue)
                                      || s.LastName.Contains(serverSidePrms.searchValue)
                                      || s.Telephone.Contains(serverSidePrms.searchValue));
            }
            var recordsFiltered = users.Count();

            users = serverSidePrms.SortingByRules(users);

            var items = users.AsNoTracking().Skip(serverSidePrms.start).Take(serverSidePrms.length).ToArray();
            var result = new ServerSidePage()
            {
                draw = serverSidePrms.draw,
                recordsTotal = recsTotal,
                recordsFiltered = recordsFiltered,
                error = string.Empty,
                data = items
            };

            return result;
        }

        // GET: api/<controller>/WithOutAdmins
        [HttpGet("WithOutAdmins")]
        public ServerSidePage GetWithOutAdmins([FromQuery]serverSideParams serverSidePrm)
        {
            var users = _dbContext.Users.Where(user => ! user.IsAdmin);
            var recsTotal = users.Count();

            if (!string.IsNullOrEmpty(serverSidePrm.searchValue))
            {
                users = users.Where(s => s.FirstName.Contains(serverSidePrm.searchValue)
                                         || s.MiddleName.Contains(serverSidePrm.searchValue)
                                         || s.LastName.Contains(serverSidePrm.searchValue)
                                         || s.Telephone.Contains(serverSidePrm.searchValue));
            }
            var recordsFiltered = users.Count();

            users = serverSidePrm.SortingByRules(users);
            var items = users.AsNoTracking().Skip(serverSidePrm.start).Take(serverSidePrm.length).ToArray();

            var result = new ServerSidePage()
            {
                draw = serverSidePrm.draw,
                recordsTotal = recsTotal,
                recordsFiltered = recordsFiltered,
                error = string.Empty,
                data = items
            };

            return result;
        }

        // GET api/<controller>/CheckUser?login=admin&password=password
        [HttpGet("CheckUser")]
        public IActionResult Get(string login, string password)
        {
            User user = _dbContext.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user == null)
                return NotFound();

            return new ObjectResult(user);
        }

        // POST api/<controller>
        [HttpPost("NewUser")]
        public IActionResult NewUser([FromQuery]User value)
        {
            if (value == null)
                return BadRequest();

            _dbContext.Users.Add(value);
            _dbContext.SaveChanges();
            return Ok();
        }

        // PUT api/<controller>
        [HttpPut("SaveUser")]
        public IActionResult SaveUser([FromQuery]User value)
        {
            var oldUser = _dbContext.Users.Find(value.Id);
            if (oldUser == null)
                return NotFound();

            oldUser.Login = value.Login;
            oldUser.Password = value.Password;
            oldUser.FirstName = value.FirstName;
            oldUser.MiddleName = value.MiddleName;
            oldUser.LastName = value.LastName;
            oldUser.IsAdmin = value.IsAdmin;
            oldUser.Telephone = value.Telephone;

            _dbContext.Users.Update(oldUser);
            _dbContext.SaveChanges();
            return Ok();
        }

        // DELETE api/<controller>/Delete?id=N
        [HttpDelete("{id}")]
        public IActionResult Delete([FromQuery]int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
                return NotFound();

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return new OkResult();
        }

        // GET api/<controller>/GetUser?id=N
        [HttpGet("{id}")]
        public IActionResult GetUser([FromQuery]int id)
        {
            User user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                return NotFound();

            return new ObjectResult(user);
        }
    }
}
