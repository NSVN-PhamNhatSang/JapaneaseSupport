using Dapper;
using JLearning.Helpers;
using GoodFood.api.Models;
using JLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MySqlConnector;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace JLearning.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly JwtSettings jwtSettings;
        private readonly IConfiguration configuration;
        public AccountController(JwtSettings jwtSettings, IConfiguration configuration)
        {
            this.jwtSettings = jwtSettings;
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLogins userLogins)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getAllUser = "Select userId,userName,userPassword,roll from users";
                IEnumerable<Users> logins = mySQLconnection.Query<Users>(getAllUser);

                var Token = new UserTokens();
                var Valid = logins.Any(x =>
                    x.userName.Equals(userLogins.userName, StringComparison.Ordinal)
                   && x.userPassword.Equals(userLogins.userPassword, StringComparison.Ordinal)
                );
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x =>
        x.userName.Equals(userLogins.userName, StringComparison.Ordinal)
        && x.userPassword.Equals(userLogins.userPassword, StringComparison.Ordinal)
    );
                    var claims = new[]
                    {   new Claim(JwtRegisteredClaimNames.Typ,user?.roll.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user?.userName),
                        new Claim(JwtRegisteredClaimNames.Sid, user?.userId),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153 - 11C1 - 4919 - 9133 - EFAF99A9B456"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        // issuer: "https://localhost:7168",
                        // audience: "https://localhost:7168",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: creds
                    );

                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    //  HttpContext.Session.SetString("JWT", tokenString);

                    return Ok(new
                    {
                        token = tokenString
                    });
                }
                else
                {
                    return BadRequest("Wrong password or account");
                }

            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e001");

        }
        [HttpPost]
        public IActionResult Register([FromBody] UserLogins userLogins)

        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getAllUser = "insert into users (userId,userName,userPassword,roll) values (@userId,@userName,@userPassword,@roll)";
                var parameter = new DynamicParameters();
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string userId = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                parameter.Add("@userId",userId);
                parameter.Add("@userName", userLogins.userName);
                parameter.Add("@userPassword", userLogins.userPassword);
                parameter.Add("@roll", 0);

                int rowefec = mySQLconnection.Execute(getAllUser, parameter);
                if (rowefec > 0)
                {
                    return Ok("Register success");
                }
                else
                {
                    return BadRequest("something wrong");
                }

            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e001");
        }
    }
}