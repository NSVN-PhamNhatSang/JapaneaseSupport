using Dapper;
using JLearning.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PostsController(IPhotoService photoService)
        {
            _photoService = photoService;
        }
        [HttpGet]
        [Route("{user_id}")]
        public IActionResult getListPosts(string user_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getListnoteWord = "Select postId,postContent,courseId,postImage,postAt from posts where userId='" + user_id + "'";
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
        public async Task<IActionResult> CreatePost(string user_Id, [FromForm] PostCreateModel postModel)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    await mySQLconnection.OpenAsync();

                    // Start a transaction
                    using (var transaction = await mySQLconnection.BeginTransactionAsync())
                    {
                        try
                        {
                            Random random = new Random();
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            int length = 10;
                            string post_Id = new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray());

                            // Upload the image if provided
                            string imgUrl = null;
                            if (postModel.PostImageFile != null)
                            {
                                var result = await _photoService.UploadPhotoAsync(postModel.PostImageFile);
                                if (result.Error != null)
                                {
                                    return BadRequest(result.Error.Message);
                                }
                                imgUrl = result.SecureUrl.AbsoluteUri;
                            }

                            string query = @"INSERT INTO posts (postId, postContent, postImage, courseId, userId, postAt) 
                                     VALUES (@postId, @postContent, @postImage, @courseId, @userId, @postAt)";
                            var parameters = new DynamicParameters();
                            parameters.Add("@postId", post_Id);
                            parameters.Add("@postContent", postModel.PostContent);
                            parameters.Add("@postImage", imgUrl);
                            parameters.Add("@courseId", postModel.CourseId);
                            parameters.Add("@userId", user_Id);
                            parameters.Add("@postAt", DateTime.Now);

                            int rowsAffected = await mySQLconnection.ExecuteAsync(query, parameters, transaction);

                            if (rowsAffected > 0)
                            {
                                await transaction.CommitAsync();
                                return Ok(new { message = "Word created successfully" });
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return BadRequest(new { message = "Something went wrong" });
                            }
                        }
                        catch (MySqlException mysqlexception)
                        {
                            await transaction.RollbackAsync();
                            if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, "e003");
                            }
                            return StatusCode(StatusCodes.Status400BadRequest, "e001");
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            return StatusCode(StatusCodes.Status400BadRequest, "e001");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
                string query = "select postId,postImage,postContent,posts.courseId,courseName,userName,users.ImgUrl,users.userLevel,rating from posts,users,courses where users.userId=posts.userId and posts.courseId=courses.courseId ";
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
        public IActionResult ImportCourse(string user_id, string course_id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    mySQLconnection.Open();

                    // Insert course into usercourses
                    string insertCourseQuery = "INSERT INTO usercourses (userId, courseId) VALUES (@userId, @courseId)";
                    var courseParams = new DynamicParameters();
                    courseParams.Add("@userId", user_id);
                    courseParams.Add("@courseId", course_id);

                    int rowsAffected = mySQLconnection.Execute(insertCourseQuery, courseParams);

                    if (rowsAffected <= 0)
                    {
                        return BadRequest("Failed to import course");
                    }

                    // Fetch wordIds associated with the course from coursedetail table
                    string fetchWordsQuery = @"
            SELECT wordId 
            FROM coursedetail 
            WHERE courseId = @courseId";
                    var wordParams = new DynamicParameters();
                    wordParams.Add("@courseId", course_id);

                    var wordIds = mySQLconnection.Query<string>(fetchWordsQuery, wordParams).ToList();

                    if (!wordIds.Any())
                    {
                        return BadRequest("No words found for the given course");
                    }

                    // Insert each wordId into userwords for the user
                    string insertWordQuery = @"
            INSERT INTO userwords (userId, wordId, learnedAt, userCourse_id, userWordStatus, learnedStatus, wordNote) 
            VALUES (@userId, @wordId, @learnedAt, @userCourse_id, @userWordStatus, @learnedStatus, @wordNote)";

                    foreach (var wordId in wordIds)
                    {
                        var wordInsertParams = new DynamicParameters();
                        wordInsertParams.Add("@userId", user_id);
                        wordInsertParams.Add("@wordId", wordId);
                        wordInsertParams.Add("@learnedAt", DateTime.Now); // You can set this to a specific datetime if needed
                        wordInsertParams.Add("@userCourse_id", $"{user_id}_{course_id}");
                        wordInsertParams.Add("@userWordStatus", 0); // Default value, adjust if necessary
                        wordInsertParams.Add("@learnedStatus", "0"); // Default value, adjust if necessary
                        wordInsertParams.Add("@wordNote", "0"); // Default value, adjust if necessary

                        mySQLconnection.Execute(insertWordQuery, wordInsertParams);
                    }

                    return Ok("Import success");
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == (int)MySqlErrorCode.DuplicateKeyEntry) // Use .Number instead of .ErrorCode for MySqlException
                {
                    return BadRequest("Duplicate entry for user and course (e003)");
                }
                else
                {
                    return BadRequest("Database error (e001)");
                }
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error (e001)");
            }
        }




    }
}
public class PostCreateModel
{
    public string PostContent { get; set; }
    public string CourseId { get; set; }
    public IFormFile? PostImageFile { get; set; }
}
