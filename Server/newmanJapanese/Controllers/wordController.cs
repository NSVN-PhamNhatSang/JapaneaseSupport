using Dapper;
using JLearning.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class wordController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public wordController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPut]
        [Route("note/{userId}/{wordId}")]
        public IActionResult noteWord(string wordId, string userId, double wordNote)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update userwords set wordNote= '" + wordNote + "' where userId='" + userId + "' and wordId='" + wordId + "'";

                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok(wordNote);
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
        [HttpPut]
        [Route("learned/{userId}/{wordId}")]
        public IActionResult learnedWord(string wordId, string userId, double learnedStatus)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update userwords set learnedStatus= '1' where userId='" + userId + "' and wordId='" + wordId + "'";

                int rowefec = mySQLconnection.Execute(query);

                if (rowefec > 0)
                {
                    return Ok(new { message = "Update successful" });
                }
                else
                {
                    return BadRequest(new { message = "Update failed" });
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
        [Route("status/{userId}/{wordId}")]
        public IActionResult statusWord(string wordId, string userId, double wordStatus)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "update userwords set userWordStatus= '" + wordStatus + "' where userId='" + userId + "' and wordId='" + wordId + "'";

                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok(wordStatus);
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

        [HttpPut]
        [Route("{word_Id}/{ status}")]
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
        [HttpGet]
        [Route("/status/{userId}/{status}")]

        public IActionResult getAllWordStatusLearn(double status, string userId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "select * from words inner join userwords on userwords.wordId  = words.wordId where userwords.userId  = '" + userId + "' and userwords.learnedStatus = '" + status + "'";
                var allWord = mySQLconnection.Query(query);
                if (allWord != null)
                {
                    return Ok(allWord);
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
        [HttpGet("filter/{userId}/{courseId}")]
        public IActionResult GetFilteredWords(string? wordNote, string? learnedStatus1, string userId, string courseId)
        {
            try
            {
                using (var connection = new MySqlConnection(DatebaseSource.name))
                {
                    connection.Open();

                    var baseQuery = @"
                SELECT DISTINCT 
                    words.wordId,
                    words.wordHiragana, 
                    words.wordMean, 
                    words.wordKanji, 
                    userwords.wordNote, 
                    words.Example 
                FROM courses
                INNER JOIN coursedetail ON courses.courseId = coursedetail.courseId
                INNER JOIN words ON words.wordId = coursedetail.wordId
                INNER JOIN userwords ON words.wordId = userwords.wordId
                WHERE userwords.userId = @userId 
                AND courses.courseId = @courseId";    

                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", courseId);
                    parameters.Add("@userId", userId);

                    var conditions = new List<string>();

                    if (!string.IsNullOrEmpty(wordNote))
                    {
                        conditions.Add("userwords.wordNote = @wordNote");
                        parameters.Add("@wordNote", wordNote);
                    }
                    if (!string.IsNullOrEmpty(learnedStatus1))
                    {
                        var learnedStatusArray = learnedStatus1.Split(',').Select(status => $"'{status}'").ToArray();
                        conditions.Add($"userwords.learnedStatus IN ({string.Join(",", learnedStatusArray)})");
                    }


                    if (conditions.Count > 0)
                    {
                        baseQuery += " AND (" + string.Join(" OR ", conditions) + ")";
                    }

                    var words = connection.Query(baseQuery, parameters);
                    return Ok(words);
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("")]

        public IActionResult getAllWord()
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "select * from words";
                var allWord = mySQLconnection.Query(query);
                if (allWord != null)
                {
                    return Ok(allWord);
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


        [HttpDelete("{courseId}/deleteWord/{wordId}")]
        public async Task<IActionResult> DeleteWord(string courseId, string wordId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    // Open the connection
                    await mySQLconnection.OpenAsync();

                    // Retrieve the word information
                    string selectWordQuery = "SELECT wordImg FROM words WHERE wordId = @WordId";
                    var word = await mySQLconnection.QueryFirstOrDefaultAsync<dynamic>(selectWordQuery, new { WordId = wordId });

                    if (word == null)
                    {
                        return NotFound("Word not found.");
                    }

                    // Delete the photo from Cloudinary
                    if (!string.IsNullOrEmpty(word.wordImg))
                    {
                        var uri = new Uri(word.wordImg);
                        var publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
                        var result = await _photoService.DeletePhotoAsync(publicId);

                        if (result.Result != "ok")
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete photo from Cloudinary.");
                        }
                    }

                    // Start a transaction
                    using (var transaction = mySQLconnection.BeginTransaction())
                    {
                        try
                        {
                            // Delete the word from the words table
                            string deleteWordQuery = "DELETE FROM words WHERE wordId = @WordId";
                            await mySQLconnection.ExecuteAsync(deleteWordQuery, new { WordId = wordId }, transaction);

                            // Delete the word from the coursedetail table
                            string deleteCourseDetailQuery = "DELETE FROM coursedetail WHERE wordId = @WordId AND courseId = @CourseId";
                            await mySQLconnection.ExecuteAsync(deleteCourseDetailQuery, new { WordId = wordId, CourseId = courseId }, transaction);

                            // Delete the word from the userwords table
                            string deleteUserWordsQuery = "DELETE FROM userwords WHERE wordId = @WordId AND userCourse_id = @CourseId";
                            await mySQLconnection.ExecuteAsync(deleteUserWordsQuery, new { WordId = wordId, CourseId = courseId }, transaction);

                            // Update the totalWord count in the courses table
                            string updateTotalWordQuery = "UPDATE courses SET totalWord = totalWord - 1 WHERE courseId = @CourseId";
                            await mySQLconnection.ExecuteAsync(updateTotalWordQuery, new { CourseId = courseId }, transaction);

                            // Update the currentWord to 0 in the usercourses table
                            string updateUserCourseQuery = "UPDATE usercourses SET currentWord = '0' WHERE courseId = @CourseId";
                            await mySQLconnection.ExecuteAsync(updateUserCourseQuery, new { CourseId = courseId }, transaction);

                            // Commit the transaction
                            transaction.Commit();

                            return Ok(new { message = "Word deleted successfully" });
                        }
                        catch
                        {
                            // Rollback the transaction if any command fails
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, mysqlexception.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpGet]
        [Route("learned/{user_id}/{word_id}")]
        public IActionResult UpdateLearnStatus(string user_id, string word_id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    mySQLconnection.Open();
                    string query = "UPDATE userwords SET learnedStatus = '1' WHERE wordId = @wordId and userId = @userId";
                    var parameters = new { wordId = word_id, userId = user_id };
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    if (rowsAffected > 0)
                    {
                        return Ok("Update success");
                    }
                    else
                    {
                        return BadRequest("No rows affected");
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        [HttpPut]
        [Route("edit/{wordId}")]
        public async Task<IActionResult> EditUserWord(string wordId, [FromForm] UserWordEditModel model)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    // Check if an image file is provided
                    if (model.ImageFile != null)
                    {
                        // Upload the image to Cloudinary or any other service
                        var result = await _photoService.UploadPhotoAsync(model.ImageFile);
                        if (result.Error != null)
                        {
                            return BadRequest(result.Error.Message);
                        }
                        string imgUrl = result.SecureUrl.AbsoluteUri; // Get the secure URL of the uploaded image

                        // Update the database with the new image URL
                        string query = @"UPDATE words SET 
                                         wordHiragana = @WordHiragana, 
                                         wordKanji = @WordKanji, 
                                         wordMean = @WordMean,
                                         Example = @WordExample,
                                         wordImg = @ImgUrl
                                         WHERE wordId = @WordId";

                        int rowefec = mySQLconnection.Execute(query, new
                        {
                            WordHiragana = model.WordHiragana,
                            WordKanji = model.WordKanji,
                            WordMean = model.WordMean,
                            WordExample = model.WordExample,
                            ImgUrl = imgUrl,
                            WordId = wordId
                        });

                        if (rowefec > 0)
                        {
                            return Ok(new { message = "Update successful" });
                        }
                        else
                        {
                            return BadRequest(new { message = "Update failed" });
                        }
                    }
                    else
                    {
                        // No image file provided, update only textual information
                        string query = @"UPDATE words SET 
                                         wordHiragana = @WordHiragana, 
                                         wordKanji = @WordKanji, 
                                         wordMean = @WordMean,
                                         Example = @WordExample
                                         WHERE wordId = @WordId";

                        int rowefec = mySQLconnection.Execute(query, new
                        {
                            WordHiragana = model.WordHiragana,
                            WordKanji = model.WordKanji,
                            WordMean = model.WordMean,
                            WordExample = model.WordExample,
                            WordId = wordId
                        });

                        if (rowefec > 0)
                        {
                            return Ok(new { message = "Update successful" });
                        }
                        else
                        {
                            return BadRequest(new { message = "Update failed" });
                        }
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        [HttpPost]
        [Route("createWord/{courseId}")]
        public async Task<IActionResult> CreateWord(string courseId, [FromForm] WordCreateModel model)
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
                            string wordId = Guid.NewGuid().ToString(); // Generate a new random wordId

                            // Upload the image if provided
                            string imgUrl = null;
                            if (model.ImageFile != null)
                            {
                                var result = await _photoService.UploadPhotoAsync(model.ImageFile);
                                if (result.Error != null)
                                {
                                    return BadRequest(result.Error.Message);
                                }
                                imgUrl = result.SecureUrl.AbsoluteUri;
                            }

                            // Insert new word into the words table
                            string insertWordQuery = @"INSERT INTO words (wordId, wordHiragana, wordKanji, wordMean, Example, wordImg) 
                                      VALUES (@WordId, @WordHiragana, @WordKanji, @WordMean, @Example, @ImgUrl)";

                            var parameters = new
                            {
                                WordId = wordId,
                                WordHiragana = model.WordHiragana,
                                WordKanji = model.WordKanji,
                                WordMean = model.WordMean,
                                Example = model.Example ?? "", // Handle nullable Example
                                ImgUrl = imgUrl
                            };

                            await mySQLconnection.ExecuteAsync(insertWordQuery, parameters, transaction);

                            // Insert into coursedetail table
                            string insertCourseDetailQuery = @"INSERT INTO coursedetail (courseId, wordId) 
                                               VALUES (@CourseId, @WordId)";

                            await mySQLconnection.ExecuteAsync(insertCourseDetailQuery, new
                            {
                                CourseId = courseId,
                                WordId = wordId,
                            }, transaction);

                            string selectUserCoursesQuery = "SELECT userId FROM usercourses WHERE courseId = @CourseId";
                            var userIds = await mySQLconnection.QueryAsync<string>(selectUserCoursesQuery, new { CourseId = courseId }, transaction);

                            string insertUserWordsQuery = @"INSERT INTO userwords (userId, wordId, learnedAt, userCourse_id, userWordStatus, learnedStatus, wordNote) 
                                            VALUES (@UserId, @WordId, @LearnedAt, @UserCourseId, @UserWordStatus, @LearnedStatus, @WordNote)";

                            foreach (var userId in userIds)
                            {
                                await mySQLconnection.ExecuteAsync(insertUserWordsQuery, new
                                {
                                    UserId = userId,
                                    WordId = wordId,
                                    LearnedAt = DateTime.Now,
                                    UserCourseId = courseId,
                                    UserWordStatus = 0,
                                    LearnedStatus = "0",
                                    WordNote = "0"
                                }, transaction);
                            }

                            // Update the totalWord count in the courses table
                            string updateTotalWordQuery = "UPDATE courses SET totalWord = totalWord + 1 WHERE courseId = @CourseId";
                            await mySQLconnection.ExecuteAsync(updateTotalWordQuery, new { CourseId = courseId }, transaction);

                            // Update the currentWord to '0' in the usercourses table
                            string updateUserCourseQuery = "UPDATE usercourses SET currentWord = '0' WHERE courseId = @CourseId";
                            await mySQLconnection.ExecuteAsync(updateUserCourseQuery, new { CourseId = courseId }, transaction);

                            // Commit the transaction
                            await transaction.CommitAsync();

                            return Ok(new { message = "Word created successfully" });
                        }
                        catch
                        {
                            // Rollback the transaction if any command fails
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, mysqlexception.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("status/{userId}/{status}")]
        public async Task<IActionResult> GetAllWordStatusLearn(string userId, int status)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = @"SELECT w.wordId, w.wordHiragana, w.wordKanji, w.wordMean, w.Example, w.wordImg, 
                                        uw.userWordStatus, uw.learnedStatus, uw.wordNote, uw.learnedAt
                                 FROM userwords uw
                                 JOIN words w ON uw.wordId = w.wordId
                                 WHERE uw.userId = @UserId AND uw.userWordStatus = @Status";

                    var allWord = await mySQLconnection.QueryAsync<UserWordDto>(query, new { UserId = userId, Status = status });

                    if (allWord != null && allWord.Any())
                    {
                        return Ok(allWord);
                    }
                    else
                    {
                        return NotFound("No words found with the given status.");
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, mysqlexception.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
    public class UserWordEditModel
    {
        public string? WordHiragana { get; set; }
        public string? WordKanji { get; set; }
        public string? WordMean { get; set; }
        public string? WordExample { get; set; }
        public IFormFile? ImageFile { get; set; } // Property for uploading image
    }
public class WordCreateModel
{
    public string WordHiragana { get; set; }
    public string WordKanji { get; set; }
    public string WordMean { get; set; }
    public string? Example { get; set; }
    public IFormFile? ImageFile { get; set; }
}
public class UserWordDto
{
    public string WordId { get; set; }
    public string WordHiragana { get; set; }
    public string WordKanji { get; set; }
    public string WordMean { get; set; }
    public string Example { get; set; }
    public string WordImg { get; set; }
    public int UserWordStatus { get; set; }
    public string LearnedStatus { get; set; }
    public string WordNote { get; set; }
    public DateTime? LearnedAt { get; set; }
}
