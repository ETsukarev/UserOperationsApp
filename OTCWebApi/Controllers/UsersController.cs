using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OTCWebApi.Models;

namespace OTCWebApi.Controllers
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

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _dbContext.Users.ToList();
        }

        // GET: api/<controller>/WithOutAdmins
        [HttpGet("WithOutAdmins")]
        public IEnumerable<User> GetWithOutAdmins()
        {
            return _dbContext.Users.Where(user => ! user.IsAdmin).ToList();
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
