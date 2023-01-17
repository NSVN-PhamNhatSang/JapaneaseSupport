using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : Controller
    {
        [HttpGet]
        [Route("{userId}")]
        public IActionResult getUserInfo(string userId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.newDbname);
                var getuserInfor = mySQLconnection.Query<User>("Select userId,username,userrating from users where userid='" + userId + "'");
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
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception e)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e004");
        }
    }
}

