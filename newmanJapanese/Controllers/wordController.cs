using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class wordController : ControllerBase
    {
        [HttpPut]
        [Route("{word_Id}/status")]
        public IActionResult rateWord(string word_Id, int status)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update words set wordStatus=" + status + " where wordId='" + word_Id + "'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok(status);
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
            return StatusCode(StatusCodes.Status400BadRequest, "e001");

        }
    }
}
