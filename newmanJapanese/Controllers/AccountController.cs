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
                string getAllUser = "Select userName,userPassword from users";
                IEnumerable<UserLogins> logins = mySQLconnection.Query<UserLogins>(getAllUser);

                var Token = new UserTokens();
                var Valid = logins.Any(x =>
                    x.UserName.Equals(userLogins.UserName, StringComparison.Ordinal)
                   && x.Password.Equals(userLogins.Password, StringComparison.Ordinal)
                );
                if (Valid)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userLogins.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:Key").Value));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: configuration.GetSection("JwtSettings:Issuer").Value,
                        audience: configuration.GetSection("JwtSettings:Audience").Value,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds
                    );

                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    HttpContext.Session.SetString("JWT", tokenString);

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
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}