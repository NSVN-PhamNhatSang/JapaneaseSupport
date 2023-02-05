using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class postsController : Controller
    {
        [HttpGet]
        [Route("{notebookId}")]
        public IActionResult GetListnoteWord(Guid notebookId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnoteWord = "Select notewordId,notebookId,kanji,cvword,hirakata,mean,example from notewords where notebookId='" + notebookId + "'";
                var ListnoteWords = mySQLconnection.Query(getListnoteWord);
                if (ListnoteWords.First() != null)
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
        public IActionResult InsertnoteWord(Guid notebookId, string kanji, string cvword, string hirakata, string mean, string example)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string insertNoteword = "insert into notewords (notewordId,notebookId,kanji,cvword,hirakata,mean,example) values (@notewordId,@notebookId,@kanji,@cvword,@hirakata,@mean,@example)";
                var parameters = new DynamicParameters();
                Guid Id = Guid.NewGuid();
                parameters.Add("@notewordId", Id);
                parameters.Add("@notebookId", notebookId);
                parameters.Add("@kanji", kanji);
                parameters.Add("@cvword", cvword);
                parameters.Add("@hirakata", hirakata);
                parameters.Add("@mean", mean);
                parameters.Add("@example", example);
                var numberRowEffect = mySQLconnection.Execute(insertNoteword, parameters);
                if (numberRowEffect > 0)
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
                string deleteWord = "delete from notewords where notewordId='" + notewordId + "'";

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
                string updateWord = "update notewords set kanji='" + kanji + "',cvword='" + cvword + "',hirakata='" + hirakata + "',mean='" + mean + "',example='" + example + "' where notewordId='" + notewordId + "'";

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
        [HttpPost]
        [Route("{post_Id}/{user_Id}")]
        public IActionResult createPost(string post_Id, string user_Id, [FromBody] Post post)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "insert into posts (postId,postContent,postImage,courseId,userId) values (@postId,@postContent,@postImage,@courseId,@userId)";
                var parameters = new DynamicParameters();
                parameters.Add("@postId", post_Id);
                parameters.Add("@postContent", post.post_content);
                parameters.Add("@postImage", post.post_image);
                parameters.Add("@courseId", post.course_id);
                parameters.Add("@userId", user_Id);
                int rowefec = mySQLconnection.Execute(query, parameters);
                if (rowefec > 0)
                {
                    return Ok("Post success");
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
        [HttpGet]
        [Route("{post_id}")]
        public IActionResult getPost(string post_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var query = "select postImage,postContent,courseId,courseName from posts,courses where posts.courseId=courses.courseId and postId='" + post_id + "'";
                var connectDb = mySQLconnection.Query<Post>(query);
                if (connectDb.First<Post>() != null)
                {
                    return Ok(connectDb);
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
        [HttpDelete]
        [Route("{post_id}")]
        public IActionResult deletePost(string post_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var query = "delete from posts where postId='" + post_id + "'";
                int rowefe = mySQLconnection.Execute(query);
                if (rowefe > 0)
                {
                    return Ok("Delete success");
                }
                else
                {
                    return BadRequest("Something wroong");
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
        [Route("{course_id}/rating")]
        public IActionResult updatePost(string course_id,int rating)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update courses set rating="+rating +" where courseId='"+course_id+"'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok(rating);
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
