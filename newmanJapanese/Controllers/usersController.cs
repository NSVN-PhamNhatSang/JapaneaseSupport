using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using JLearning.Controllers;
using GoodFood.api.Models;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

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
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var getuserInfor = mySQLconnection.Query<User>("Select username,userlevel,usercategory from users where userId='"+userId+"'");
                if (getuserInfor.First<User>() != null)
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
        [Route("{userId}")]
        public IActionResult  setUserInfor(string userId, [FromBody] UserPut infor)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var numberRowEffect = mySQLconnection.Execute("update users set userLevel="+infor.level+",usercategory='"+infor.category+"' where userId='"+userId+"'");
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

        [HttpPut]
        [Route("change/{user_id}")]
        public IActionResult adminSetUserInfor(string user_id,[FromBody] UserLogins userLogins)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update users set userName='"+userLogins.userName+"' , userPassword='"+userLogins.userPassword+"' where userId='"+user_id+"'";
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
                string query = "delete from users where userId='"+user_id+"'";
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


    }
}

