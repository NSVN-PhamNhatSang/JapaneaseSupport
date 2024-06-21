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
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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
                        issuer: "https://localhost:7168",
                        audience: "https://localhost:7168",
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
                string insertUserQuery = "INSERT INTO users (userId, userName, userPassword, roll, createdAt, ImgUrl) VALUES (@userId, @userName, @userPassword, @roll, @createdAt, @ImgUrl)";
                var parameter = new DynamicParameters();
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string userId = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                parameter.Add("@userId", userId);
                parameter.Add("@userName", userLogins.userName);
                parameter.Add("@userPassword", userLogins.userPassword);
                parameter.Add("@roll", 0);
                parameter.Add("@createdAt", DateTime.Now);

                // Set the default avatar URL
                string defaultAvatarUrl = "https://th.bing.com/th/id/OIP.7i35GvRSp092_L3KWHr4jgHaHv?rs=1&pid=ImgDetMain"; // Change this to your default avatar URL
                parameter.Add("@ImgUrl", defaultAvatarUrl);

                int rowefec = mySQLconnection.Execute(insertUserQuery, parameter);
                if (rowefec > 0)
                {
                    string insertUserDetailQuery = "INSERT INTO userdetail (userId, email) VALUES (@userId, @userId)";
                    var userDetailParams = new DynamicParameters();
                    userDetailParams.Add("@userId", userId);
                    userDetailParams.Add("@email", "");
                    int userDetailRowsAffected = mySQLconnection.Execute(insertUserDetailQuery, userDetailParams);
                    if (userDetailRowsAffected > 0)
                    {
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Typ, "0"), // Default roll value
                    new Claim(JwtRegisteredClaimNames.Sub, userLogins.userName),
                    new Claim(JwtRegisteredClaimNames.Sid, userId),
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: "https://localhost:7168",
                            audience: "https://localhost:7168",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(20),
                            signingCredentials: creds
                        );

                        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                        return Ok(new
                        {
                            message = "Register success",
                            token = tokenString
                        });
                    }
                    else
                    {
                        return BadRequest(new { error = "Register failed" });
                    }
                }
                else
                {
                    return BadRequest(new { error = "Something went wrong" });
                }
            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { error = "Username already exists. Please choose a different username." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later." });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later." });
            }
        }


        [HttpPost("{userName}")]
        public IActionResult CheckUserNameExist(string userName)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getUser = "Select count(*) from users where userName = @userName";
                var parameter = new { userName };
                int count = mySQLconnection.QueryFirstOrDefault<int>(getUser, parameter);
                if (count > 0)
                {
                    return Ok(true); // Tên đăng nhập đã tồn tại
                }
                else
                {
                    return Ok(false); // Tên đăng nhập chưa tồn tại
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }

}