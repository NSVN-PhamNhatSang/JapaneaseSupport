using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace newmanJapanese.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class wordbookController : Controller
    {   
        [HttpPost]
        [Route("{userId}")]
        public IActionResult InsertWordBook(string userId,string kanji,string cvword,string hirakata,string mean,string example)
        {
            try
            {


                string connectionString = "Server=localhost;Port=3306;Database=learning;Uid=root;Pwd=yasuo1234gg";
                var mySQLconnection = new MySqlConnection(connectionString);
                string InsertWord = "insert into wordbooks (wordId,kanji,cvword,hirakata,mean,example) values (@wordId,@kanji,@cvword,@hirakata,@mean,@example)";
                Guid id = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@wordId", id);
                parameters.Add("@kanji", kanji);
                parameters.Add("@cvword", cvword);
                parameters.Add("@hirakata", hirakata);
                parameters.Add("@mean", mean);
                parameters.Add("@example", example);

                var numberroweffect = mySQLconnection.Execute(InsertWord, parameters);
                if (numberroweffect > 0)
                {
                    return Ok("Word created");
                }
                else
                {
                    return BadRequest("failure create");
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
