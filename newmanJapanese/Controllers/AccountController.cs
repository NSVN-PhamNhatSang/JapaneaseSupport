using Dapper;
using JLearning.Helpers;
using GoodFood.api.Models;
using JLearning.Helpers;
using JLearning.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace JLearning.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        [HttpPost]
        public IActionResult Login(UserLogins userLogins)

        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getAllUser = "Select * from users";
                IEnumerable<Users> logins = mySQLconnection.Query<Users>(getAllUser);

                var Token = new UserTokens();
                var Valid = logins.Any(x =>
                    x.username.Equals(userLogins.UserName, StringComparison.Ordinal)
                   && x.password.Equals(userLogins.Password, StringComparison.Ordinal)
                );
                if (Valid)
                {
                    return Ok("Login success");
                }
                else
                {
                    return BadRequest("wrong password or account");
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
