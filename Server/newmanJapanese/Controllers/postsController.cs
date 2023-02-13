using Dapper;
using Microsoft.AspNetCore.Authorization;
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
        [Route("{user_id}")]
        public IActionResult getListPosts(string user_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnoteWord = "Select postId,postContent,courseId,postImage from posts where userId='" + user_id + "'";
                var ListnoteWords = mySQLconnection.Query(getListnoteWord);
                if (ListnoteWords.First() != null)
                {
                    return Ok(ListnoteWords);
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
        [Route("{user_Id}")]
        public IActionResult createPost(string user_Id, [FromBody] Post post)
        {
            try
            {
              
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);

               Random random= new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string post_Id = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string query = "insert into posts (postId,postContent,postImage,courseId,userId) values (@postId,@postContent,@postImage,@courseId,@userId)";
                var parameters = new DynamicParameters();
                parameters.Add("@postId", post_Id);
                parameters.Add("@postContent", post.postContent);
                parameters.Add("@postImage", post.postImage);
                parameters.Add("@courseId", post.courseId);
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
        [Route("aPost/{post_id}")]
        public IActionResult getPost(string post_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                var query = "select postId,postImage,postContent,posts.courseId,courseName,userName from posts,courses,users where posts.courseId=courses.courseId and users.userId=posts.userId and postId='" + post_id + "'";
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
        [Route("{course_id}/{rating}")]
        public IActionResult updatePost(string course_id,int rating)
        {
            try 
            {
                
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string numberofRatings = "select rating,numberRating from courses where courseId='" + course_id + "'";
                IEnumerable<CourseRating> number = mySQLconnection.Query<CourseRating>(numberofRatings);
                double ratings = number.First<CourseRating>().rating;
                int numberOfRatings = number.First<CourseRating>().numberRating;
                double lasRating = ((ratings * numberOfRatings) + rating) / (numberOfRatings + 1);



                string query = "update courses set rating="+lasRating +", numberRating="+ (numberOfRatings + 1) + " where courseId='"+course_id+"'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec>0)
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
        
        [HttpGet]
        [Route("")]
        public IActionResult getAllPost()
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "select postId,postImage,postContent,posts.courseId,courseName,userName,rating from posts,users,courses where users.userId=posts.userId and posts.courseId=courses.courseId ";
                var connectDb = mySQLconnection.Query(query);
                if (connectDb.First() != null)
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

        [HttpPost]
        [Route("{user_id}/{course_id}")]
        public IActionResult importCourse(string user_id, string course_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "insert into usercourses (userId,courseId) values (@userId,@courseId)";
                var parameters = new DynamicParameters();
                parameters.Add("@userId", user_id);
                parameters.Add("@courseId", course_id);
                int rowefec = mySQLconnection.Execute(query, parameters);
                if (rowefec > 0)
                {
                    return Ok("Import success");
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
            catch (Exception)
            {
                StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e001");
        }



    }
}
