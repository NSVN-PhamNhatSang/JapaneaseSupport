using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notebookController : Controller
    {
        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetListnoteBook(Guid userId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnotebook = "Select notebookId,userId,name,totalWord,createAt,lastLearnAt from notebooks where userId='" + userId + "'";
                var ListnoteBook = mySQLconnection.Query<NoteBook>(getListnotebook);
                if (ListnoteBook.First<NoteBook>() != null)
                {
                    return Ok(ListnoteBook);
                }
                else
                {
                    return BadRequest("You got some wrong or don't have any notebook");
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
        [Route("{userId}")]
        public IActionResult CreateNotebook(Guid userId,string name,int totalWord)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string InsertnoteBook = "insert into notebooks (notebookId,userId,name,totalWord,createAt,lastLearnAt) values (@notebookId,@userId,@name,@totalWord,@createAt,@lastLearnAt) ";
                Guid Id= Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@notebookId", Id);
                parameters.Add("@userId", userId);
                parameters.Add("@name", name);
                parameters.Add("@totalWord", totalWord);
                parameters.Add("@createAt", DateTime.Now);
                parameters.Add("@lastLearnAt",DateTime.Now);
                int numberRowAffect = mySQLconnection.Execute(InsertnoteBook,parameters);
                if (numberRowAffect>0)
                {
                    return Ok("Created");
                }
                else
                {
                    return BadRequest("You got some wrong");
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
        [HttpDelete]
        [Route("{notebookId}")]
        public IActionResult DeleteNotebook(Guid notebookId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string DeletenoteBook = " delete from notebooks where notebookId='"+notebookId+"'";
                
                int numberRowAffect = mySQLconnection.Execute(DeletenoteBook);
                if (numberRowAffect > 0)
                {
                    return Ok("Deleted");
                }
                else
                {
                    return BadRequest("You got some wrong");
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
        [HttpPut]
        [Route("{notebookId}")]
        public IActionResult UpdateNotebook(Guid notebookId,string name)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string UpdatenoteBook = " update notebooks set name='"+name+"' where notebookId='"+notebookId+"'";

                int numberRowAffect = mySQLconnection.Execute(UpdatenoteBook);
                if (numberRowAffect > 0)
                {
                    return Ok("Updated");
                }
                else
                {
                    return BadRequest("You got some wrong");
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
