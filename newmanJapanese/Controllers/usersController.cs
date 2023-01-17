using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using JLearning.Controllers;

namespace newmanJapanese.Controllers
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
                var getuserInfor = mySQLconnection.Query<User>("Select userId,username,userrating from users where userId='12dwqd'");
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
            catch (Exception)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e004");
        }



        [HttpPut]
        [Route("{userId}")]
        public IActionResult  setUserInfor(string userId, [FromBody] UserPut infor)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.newDbname);
                var numberRowEffect = mySQLconnection.Execute("update users set userLevel="+infor.level+",category='"+infor.category+"' where userId='"+userId+"'");
                if (numberRowEffect >0)
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

    }
}

