using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using JLearning.Controllers;
using GoodFood.api.Models;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using JLearning.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using JLearning.Data;
using JLearning.Services;
using Newtonsoft.Json;
using System.Text;

namespace newmanJapanese.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly WebContext _context;

        public usersController(WebContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }


        [HttpGet]
        [Route("{userId}")]
        public IActionResult getUserInfo(string userId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var query = "SELECT username, userlevel, usercategory,ImgUrl,CreatedAt,userPoint FROM users WHERE userId = @UserId";
                var getuserInfor = mySQLconnection.Query<User>(query, new { UserId = userId }).FirstOrDefault();

                if (getuserInfor != null)
                {
                    return Ok(getuserInfor);
                }
                else
                {
                    return BadRequest("Something wrong");
                }
            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        [HttpGet]
        [Route("")]

        public IActionResult getAllUsers()
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var getuserInfor = mySQLconnection.Query("Select * from users ");
                if (getuserInfor.First() != null)
                {
                    return Ok(getuserInfor);
                }
                else
                {
                    return BadRequest("Something wrong");
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
            return StatusCode(StatusCodes.Status400BadRequest, "e004");

        }

        [HttpPut]
        [Route("info/{userId}")]
        public IActionResult UpdateUserDetail(string userId, [FromBody] UserDetailUpdate userDetails)
        {
            // Validate gender field
            var validGenders = new List<string> { "Male", "Female", "Other" };
            if (!validGenders.Contains(userDetails.Gender))
            {
                return BadRequest(new { error = "Invalid gender value. Allowed values are: Male, Female, Other." });
            }

            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string updateQuery = @"
                UPDATE userdetail 
                SET email = @Email, address = @Address, gender = @Gender 
                WHERE userId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@Email", userDetails.Email);
                parameters.Add("@Address", userDetails.Address);
                parameters.Add("@Gender", userDetails.Gender);
                parameters.Add("@UserId", userId);

                int rowsAffected = mySQLconnection.Execute(updateQuery, parameters);

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User details updated successfully" });
                }
                else
                {
                    return BadRequest(new { error = "Update failed, user not found or no changes made" });
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database error. Please try again later." });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unknown error occurred. Please try again later." });
            }
        }


        [HttpPut]
        [Route("{userId}")]
        public IActionResult setUserInfor(string userId, [FromBody] UserPut infor)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var numberRowEffect = mySQLconnection.Execute("update users set userLevel=" + infor.level + ",usercategory='" + infor.category + "' where userId='" + userId + "'");
                if (numberRowEffect > 0)
                {

                    return Ok("Value is updated");
                }
                else
                {
                    return BadRequest("Something wrong");
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
            catch (Exception e)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e004");
        }

        [HttpPut]
        [Route("change/{user_id}")]
        public IActionResult adminSetUserInfor(string user_id, [FromBody] UserLogins userLogins)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update users set userName='" + userLogins.userName + "' , userPassword='" + userLogins.userPassword + "' where userId='" + user_id + "'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok("Update success");
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
            catch (Exception e)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e004");
        }
        [HttpDelete]
        [Route("{user_id}")]

        public IActionResult deleteUser(string user_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "delete from users where userId='" + user_id + "'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok("delete success");
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
            catch (Exception e)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e004");
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost("{id}/avatar")]
        public async Task<IActionResult> UpdateAvatar(string id, IFormFile file)
        {
            // Upload the image to Cloudinary
            var result = await _photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var imageUrl = result.SecureUrl.AbsoluteUri;
            // Find the user in the database
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user with the new image URL
            user.ImgUrl = imageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            // Save changes to the database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(201);
        }
        [HttpPut("level/{userId}/{level}")]
        public async Task<IActionResult> UpdateLevel(string userId, int level)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                await mySQLconnection.OpenAsync();

                var updateQuery = @"
                UPDATE users 
                SET userLevel = @Level, updatedAt = @UpdatedAt 
                WHERE userId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@Level", level);
                parameters.Add("@UpdatedAt", DateTime.Now);
                parameters.Add("@UserId", userId);

                int rowsAffected = await mySQLconnection.ExecuteAsync(updateQuery, parameters);

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User level updated successfully" });
                }
                else
                {
                    return BadRequest(new { error = "Update failed, user not found or no changes made" });
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database error. Please try again later." });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unknown error occurred. Please try again later." });
            }
        }

        [HttpPut("userCategory/{userId}")]
        public IActionResult UpdateUserCategory(string userId, [FromBody] UpdateUserCategoryRequest updateUserCategoryRequest)
        {
            if (updateUserCategoryRequest.UserCategory == null || updateUserCategoryRequest.UserCategory.Count == 0)
            {
                return BadRequest(new { error = "UserCategory list is required." });
            }

            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                mySQLconnection.Open();

                var userCategoryJson = Newtonsoft.Json.JsonConvert.SerializeObject(updateUserCategoryRequest.UserCategory);

                var updateQuery = @"
                UPDATE users 
                SET usercategory = @UserCategory, updatedAt = @UpdatedAt 
                WHERE userId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@UserCategory", userCategoryJson);
                parameters.Add("@UpdatedAt", DateTime.Now);
                parameters.Add("@UserId", userId);

                int rowsAffected = mySQLconnection.Execute(updateQuery, parameters);

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User category updated successfully" });
                }
                else
                {
                    return BadRequest(new { error = "Update failed, user not found or no changes made" });
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database error. Please try again later." });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unknown error occurred. Please try again later." });
            }
        }
    }

}

public class UserDetailUpdate
{
    public string Email { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
}

public class UpdateUserCategoryRequest
{
    public List<string> UserCategory { get; set; }
}