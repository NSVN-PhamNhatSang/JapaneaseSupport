using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class commentController : Controller
    {
        [HttpGet]
        [Route("{post_id}")]
        public IActionResult GetListnoteBook(string post_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnotebook = "Select commentId,commentText,comment.userId, users.userName,users.ImgUrl from comment join users on comment.userId = users.userId  where postId='" + post_id + "'";
                var ListnoteBook = mySQLconnection.Query<Comment>(getListnotebook);
                if (ListnoteBook.First<Comment>() != null)
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

        
        [HttpPut]
        [Route("{comment_id}")]
        public IActionResult rateWord(string comment_id, [FromBody] AllPost comment)
        { 
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update comment set commentText='"+comment.comment+"' where commentId='"+comment_id+"'";
                int rowefe=mySQLconnection.Execute(query);
                if(rowefe > 0)
                {
                    return Ok("Update success");
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
        [HttpPost]
        [Route("{user_id}/{post_id}")]

        public IActionResult Addcomment(string user_id, string post_id, [FromBody] AllPost comment)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "insert into comment (commentId,postId,userId,commentText) values (@commentId,@postId,@userId,@commentText)";
                var parameters = new DynamicParameters();
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string id = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string commentId = id;
                parameters.Add("@commentId", commentId);
                parameters.Add("@postId", post_id);
                parameters.Add("@userId", user_id);
                parameters.Add("@commentText", comment.comment);
                int rowefec = mySQLconnection.Execute(query, parameters);
                if (rowefec > 0)
                {
                    return Ok("Add success");
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
