using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notewordController : Controller
    {
        [HttpGet]
        [Route("{notebookId}")]
        public IActionResult GetListnoteWord(Guid notebookId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnoteWord = "Select notewordId,notebookId,kanji,cvword,hirakata,mean,example from notewords where notebookId='" + notebookId + "'";
                var ListnoteWords = mySQLconnection.Query<NoteWord>(getListnoteWord);
                if (ListnoteWords.First<NoteWord>() != null)
                {
                    return Ok(ListnoteWords);
                }
                else
                {
                    return BadRequest("You got some wrong or don't have any notewords");
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
        [Route("{notebookId}")]
        public IActionResult InsertnoteWord(Guid notebookId,string kanji,string cvword,string hirakata,string mean,string example)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string insertNoteword = "insert into notewords (notewordId,notebookId,kanji,cvword,hirakata,mean,example) values (@notewordId,@notebookId,@kanji,@cvword,@hirakata,@mean,@example)";
                var parameters = new DynamicParameters();
                Guid Id = Guid.NewGuid();
                parameters.Add("@notewordId", Id);
                parameters.Add("@notebookId",notebookId);
                parameters.Add("@kanji", kanji);
                parameters.Add("@cvword", cvword);
                parameters.Add("@hirakata", hirakata);
                parameters.Add("@mean", mean);
                parameters.Add("@example", example);
                var numberRowEffect= mySQLconnection.Execute(insertNoteword,parameters);
                if (numberRowEffect>0)
                {
                    return Ok("Created");
                }
                else
                {
                    return BadRequest("You got some wrong ");
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
        [Route("{notewordId}")]
        public IActionResult RewriteWord(Guid notewordId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string deleteWord = "delete from notewords where notewordId='"+notewordId+"'";
               
                var numberRowEffect = mySQLconnection.Execute(deleteWord);
                if (numberRowEffect > 0)
                {
                    return Ok("Deleted");
                }
                else
                {
                    return BadRequest("You got some wrong ");
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
        [Route("{notewordId}")]
        public IActionResult RewriteWord(Guid notewordId, string kanji, string cvword, string hirakata, string mean, string example)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string updateWord = "update notewords set kanji='"+kanji+"',cvword='"+cvword+"',hirakata='"+hirakata+"',mean='"+mean+"',example='"+example+"' where notewordId='"+notewordId+"'";
                
                var numberRowEffect = mySQLconnection.Execute(updateWord);
                if (numberRowEffect > 0)
                {
                    return Ok("Updated");
                }
                else
                {
                    return BadRequest("You got some wrong ");
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
