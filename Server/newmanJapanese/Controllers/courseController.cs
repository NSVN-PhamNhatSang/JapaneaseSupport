using CloudinaryDotNet.Actions;
using Dapper;
using JLearning.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Reflection.Emit;
using System.Xml.Linq;
using OfficeOpenXml;
using ClosedXML.Excel;
using JLearning.Dto;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml.Packaging.Ionic.Zlib;

namespace newmanJapanese.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class courseController : Controller
    {
        [HttpGet]
        [Route("others/{userId}")]
        // [Authorize]
        public IActionResult GetCoursesCreatedByOthers(string userId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourses = @"
                SELECT c.courseId, c.courseName, c.level, c.category, c.creatorId, c.createdAt, uc.courseNote, u.userName AS CreatorName, u.ImgUrl AS CreatorImgUrl
                FROM courses c
                JOIN usercourses uc ON c.courseId = uc.courseId
                JOIN users u ON c.creatorId = u.userId
                WHERE uc.userId = @UserId
                  AND c.creatorId != @UserId
 ORDER BY   uc.learnedAt";

                    var courses = mySQLconnection.Query<dynamic>(getCourses, new { UserId = userId }).ToList();

                    var result = courses.Select(course => new Course
                    {
                        CreatorId = course.creatorId,
                        CourseId = course.courseId,
                        CourseName = course.courseName,
                        Level = course.level,
                        Category = string.IsNullOrEmpty(course.category) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(course.category),
                        CreatorName = course.CreatorName,
                        CreatorImgUrl = course.CreatorImgUrl,
                        CreatedAt = course.createdAt,
                        CourseNote = course.courseNote  // Added courseNote field
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No courses found for the given user.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpGet]
        [Route("coursewords/{courseId}")]
        // [Authorize]
        public IActionResult GetWordsByCourseId(string courseId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getWords = @"
            SELECT w.wordId, w.wordMean, w.wordHiragana, w.wordKanji, w.Example, w.wordImg
            FROM coursedetail cd
            INNER JOIN words w ON cd.wordId = w.wordId
            WHERE cd.courseId = @CourseId";

                    var words = mySQLconnection.Query<dynamic>(getWords, new { CourseId = courseId }).ToList();

                    var result = words.Select(word => new JLearning.Controllers.Word
                    {
                     
                        wordMean = word.wordMean,
                        wordHiragana = word.wordHiragana,
                        wordKanji = word.wordKanji,
                        Example = word.Example,
                      
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No words found for the given course.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.ErrorCode)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        return StatusCode(StatusCodes.Status400BadRequest, "e003");
                    default:
                        return StatusCode(StatusCodes.Status400BadRequest, "e001");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }


        [HttpGet]
        [Route("courseNote/{userId}")]
        // [Authorize]
        public IActionResult GetCoursesNote(string userId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourses = @"
                SELECT c.courseId, c.courseName, c.level, c.category, c.creatorId, c.createdAt, uc.courseNote, u.userName AS CreatorName, u.ImgUrl AS CreatorImgUrl
                FROM courses c
                JOIN usercourses uc ON c.courseId = uc.courseId
                JOIN users u ON c.creatorId = u.userId
                WHERE uc.userId = @UserId
                  AND uc.courseNote = 1
                 ORDER BY  uc.learnedAt

";

                    var courses = mySQLconnection.Query<dynamic>(getCourses, new { UserId = userId }).ToList();

                    var result = courses.Select(course => new Course
                    {
                        CreatorId = course.creatorId,
                        CourseId = course.courseId,
                        CourseName = course.courseName,
                        Level = course.level,
                        Category = string.IsNullOrEmpty(course.category) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(course.category),
                        CreatorName = course.CreatorName,
                        CreatorImgUrl = course.CreatorImgUrl,
                        CreatedAt = course.createdAt,
                        CourseNote = course.courseNote  // Added courseNote field
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No courses found for the given user.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpGet]
        [Route("created/{user_Id}")]
        // [Authorize]
        public IActionResult GetCoursesCreatedByUser(string user_Id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourses = @"
                SELECT c.courseId, c.courseName, c.level, c.category, c.creatorId, c.createdAt, u.userName AS CreatorName, u.ImgUrl AS CreatorImgUrl, uc.courseNote
                FROM courses c
                LEFT JOIN usercourses uc ON c.courseId = uc.courseId
AND c.creatorId = uc.userId
                JOIN users u ON c.creatorId = u.userId
                WHERE c.creatorId = @UserId
                ORDER BY   uc.learnedAt
                ";

                    var courses = mySQLconnection.Query<dynamic>(getCourses, new { UserId = user_Id }).ToList();

                    var result = courses.Select(course => new Course
                    {
                        CreatorId = course.creatorId,
                        CourseId = course.courseId,
                        CourseName = course.courseName,
                        Level = course.level,
                        Category = string.IsNullOrEmpty(course.category) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(course.category),
                        CreatorName = course.CreatorName,
                        CreatorImgUrl = course.CreatorImgUrl,
                        CreatedAt = course.createdAt,
                        CourseNote = course.courseNote  // Added courseNote field
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No courses found for the given user.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }


        [HttpGet]
        [Route("{user_Id}")]
        public IActionResult Getcourse(string user_Id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourses = @"
                SELECT uc.courseId, c.courseName, c.level, c.category, c.createdAt, c.totalWord, c.creatorId, u.userName AS CreatorName, u.ImgUrl AS CreatorImgUrl, uc.courseNote
                FROM usercourses uc
                INNER JOIN courses c ON uc.courseId = c.courseId
                INNER JOIN users u ON c.creatorId = u.userId
                WHERE uc.userId = @UserId";

                    var courses = mySQLconnection.Query<dynamic>(getCourses, new { UserId = user_Id }).ToList();

                    var result = courses.Select(course => new Course
                    {
                        CreatorId = course.creatorId,
                        CourseId = course.courseId,
                        CourseName = course.courseName,
                        Level = course.level,
                        Category = string.IsNullOrEmpty(course.category) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(course.category),
                        CreatorName = course.CreatorName,
                        CreatorImgUrl = course.CreatorImgUrl,
                        CreatedAt = course.createdAt,
                        TotalWord = course.totalWord,
                        CourseNote = course.courseNote
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No courses found for the given user.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }




        [HttpGet]
        [Route("info/{userId}/{course_Id}")]
        // [Authorize]
        public IActionResult GetCourseInfo(string course_Id, string userId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourseInfo = @"
                SELECT c.courseId, c.courseName, c.rating, c.category, c.createdAt, c.creatorId, uc.progress, uc.currentWord, uc.coursePoint, c.level, c.totalWord
                FROM courses c
                INNER JOIN usercourses uc ON c.courseId = uc.courseId
                WHERE c.courseId = @CourseId
                AND uc.userId = @UserId";

                    var courseDTO = mySQLconnection.QuerySingleOrDefault<CourseDto>(getCourseInfo, new { CourseId = course_Id, UserId = userId });

                    if (courseDTO != null)
                    {
                        var course = new Course
                        {
                            CourseId = courseDTO.CourseId,
                            CourseName = courseDTO.CourseName,
                            Rating = courseDTO.Rating,
                            CreatedAt = courseDTO.CreatedAt,
                            CreatorId = courseDTO.CreatorId,
                            Progress = courseDTO.Progress,
                            CurrentWord = courseDTO.CurrentWord,
                            CoursePoint = courseDTO.CoursePoint,
                            Level = courseDTO.Level,
                            TotalWord = courseDTO.TotalWord,
                            Category = !string.IsNullOrEmpty(courseDTO.Category) ? JsonConvert.DeserializeObject<List<string>>(courseDTO.Category) : new List<string>()
                        };

                        return Ok(course);
                    }
                    else
                    {
                        return NotFound("Course not found.");
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
        [Route("{user_Id}")]
        public IActionResult createCourse(string user_Id, [FromBody] CourseCreateDto courseItem)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string id = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string name = courseItem.Name;
                int level = courseItem.Level;
                string categoryJson = JsonConvert.SerializeObject(courseItem.Category); // Serialize list to JSON string

                var query = "INSERT INTO courses (courseId, category, level, courseName, creatorId, createdAt) " +
                            "VALUES (@courseId, @category, @level, @courseName, @creatorId, @createdAt)";

                var parameters = new DynamicParameters();
                parameters.Add("@courseId", id);
                parameters.Add("@category", categoryJson); // Insert JSON string into JSON column
                parameters.Add("@level", level);
                parameters.Add("@courseName", name);
                parameters.Add("@creatorId", user_Id);
                parameters.Add("@createdAt", DateTime.Now);

                int rowsAffected = mySQLconnection.Execute(query, parameters);

                var query2 = "INSERT INTO usercourses (courseId, userId) VALUES (@courseId, @userId)";
                var parameters2 = new DynamicParameters();
                parameters2.Add("@courseId", id);
                parameters2.Add("@userId", user_Id);

                int rowsAffected2 = mySQLconnection.Execute(query2, parameters2);

                if (rowsAffected > 0 && rowsAffected2 > 0)
                {
                    return Ok(new { courseId = id, message = "Create course success" });
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }


        [HttpDelete]
        [Route("{course_Id}")]
        public IActionResult DeleteCourse(string course_Id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "Delete from courses where courseId='" + course_Id + "'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok(new { message = "Update successful" });
                }
                else
                {
                    return BadRequest(new { message = "Update failed" });
                }
                //using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                //{
                //    mySQLconnection.Open();

                //    string query = "DELETE FROM courses WHERE courseId = @courseId";
                //    using (var command = new MySqlCommand(query, mySQLconnection))
                //    {
                //        command.Parameters.AddWithValue("@courseId", course_Id);
                //        int rowsAffected = command.ExecuteNonQuery();

                //        if (rowsAffected > 0)
                //        {
                //            return Ok(new { message = "Delete successful" });
                //        }
                //        else
                //        {
                //            return BadRequest(new { message = "Delete failed, course not found" });
                //        }
                //    }
            //}
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
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }

            [HttpDelete]
        [Route("usercourse/{userId}/{course_Id}")]

        public IActionResult deleteUserCourse(string course_Id, string userId)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "Delete from usercourses where courseId='" + course_Id + "' and userId= '" + userId + "'";
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
        [HttpGet]
        [Route("join/{user_Id}/{course_Id}")]
        public IActionResult GetCourseInfor(string course_Id, string user_Id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = @"
                SELECT 
                    words.wordId, 
                    wordHiragana, 
                    wordMean, 
                    wordKanji, 
                    words.wordImg,
                    wordNote, 
                    Example 
                FROM 
                    courses 
                INNER JOIN 
                    coursedetail ON courses.courseId = coursedetail.courseId 
                INNER JOIN 
                    words ON words.wordId = coursedetail.wordId 
                INNER JOIN 
                    userwords ON words.wordId = userwords.wordId 
                WHERE 
                    courses.courseId = @course_Id and
                    userwords.userId = @user_Id
                ";

                    var parameters = new { course_Id = course_Id, user_Id = user_Id };
                    IEnumerable<CourseInfor> connectDB = mySQLconnection.Query<CourseInfor>(query, parameters);

                    if (connectDB.Any())
                    {
                        return Ok(connectDB);
                    }
                    else
                    {
                        return BadRequest("No records found");
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


        [HttpGet]
        [Route("course/userInfo/{course_Id}")]


        public IActionResult GetUserInforOfCourse(string course_Id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = @"
                    SELECT users.userName, users.userId, users.ImgUrl
                    FROM users
                    INNER JOIN courses ON users.userId = courses.creatorId
                    INNER JOIN usercourses ON usercourses.courseId = courses.courseId
                    WHERE usercourses.courseId = @CourseId";

                    var parameters = new { CourseId = course_Id };
                    var userInfo = mySQLconnection.Query<User>(query, parameters).FirstOrDefault();

                    if (userInfo != null)
                    {
                        return Ok(userInfo);
                    }
                    else
                    {
                        return NotFound("User information not found for the given course ID.");
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                // Log the exception details here for further analysis
                if (mysqlexception.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Duplicate entry error.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception ex)
            {
                // Log the exception details here for further analysis
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpPut]
        [Route("learnedAt/{course_id}")]
        public IActionResult courseLearnedAt(string course_id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "UPDATE usercourses uc ON uc.courseId = @courseId SET uc.learnedAt = @learnedAt ";
                var parameters = new DynamicParameters();
                parameters.Add("@courseId", course_id);
                parameters.Add("@learnedAt", DateTime.Now);
                int rowefe = mySQLconnection.Execute(query, parameters);
                if (rowefe > 0)
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

        [HttpPut]
        [Route("currentWord/{user_id}/{course_id}/{currentWord}")]
        public IActionResult CurrentWord(string user_id, string course_id, int currentWord)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE usercourses uc SET uc.currentWord = @currentWord WHERE uc.courseId = @courseId AND uc.userId = @userId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@userId", user_id);
                    parameters.Add("@currentWord", currentWord);
                    mySQLconnection.Open();
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
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
        [HttpPut]
        [Route("progress/{user_id}/{course_id}/{progress}")]
        public IActionResult UpdateProgress(string user_id, string course_id, int progress)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE usercourses SET progress = @progress WHERE courseId = @courseId AND userId = @userId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@userId", user_id);
                    parameters.Add("@progress", progress);

                    mySQLconnection.Open(); // Ensure the connection is opened before executing the query
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    // Log the number of rows affected by the SQL update operation
                    Console.WriteLine("Rows affected:", rowsAffected);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                // Log MySQL exception details
                Console.WriteLine("MySQL exception:", mysqlexception);

                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "e003" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine("Exception:", ex);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
        }

        [HttpPut]
        [Route("point/{user_id}/{course_id}/{point}")]
        public IActionResult UpdatePoint(string user_id, string course_id, int point)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE usercourses SET coursePoint = @point WHERE courseId = @courseId AND userId = @userId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@userId", user_id);
                    parameters.Add("@point", point);

                    mySQLconnection.Open(); // Ensure the connection is opened before executing the query
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    // Log the number of rows affected by the SQL update operation
                    Console.WriteLine("Rows affected:", rowsAffected);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                // Log MySQL exception details
                Console.WriteLine("MySQL exception:", mysqlexception);

                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "e003" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine("Exception:", ex);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
        }
        [HttpPost]
        [Route("create/{user_id}/{course_id}")]
        public IActionResult CreateWord(string user_id, string course_id, [FromBody] JLearning.Controllers.Word word)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    mySQLconnection.Open();
                    using (var transaction = mySQLconnection.BeginTransaction())
                    {
                        try
                        {
                            Random random = new Random();
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            int length = 10;
                            string id = new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray());

                            // Insert into words table
                            string query = "INSERT INTO words (wordId, wordMean, wordKanji, wordHiragana, example) VALUES (@wordId, @wordMean, @wordKanji, @wordHiragana, @example)";
                            var parameters = new DynamicParameters();
                            parameters.Add("@wordId", id);
                            parameters.Add("@wordMean", word.wordMean);
                            parameters.Add("@wordKanji", word.wordKanji);
                            parameters.Add("@wordHiragana", word.wordHiragana);
                            parameters.Add("@example", word.Example); // This can be null
                            int rowefe = mySQLconnection.Execute(query, parameters, transaction);

                            // Insert into coursedetail table with default courseStatus
                            string query2 = "INSERT INTO coursedetail (courseId, wordId, courseStatus) VALUES (@courseId, @wordId, @courseStatus)";
                            var parameters2 = new DynamicParameters();
                            parameters2.Add("@wordId", id);
                            parameters2.Add("@courseId", course_id);
                            parameters2.Add("@courseStatus", 0); // Default value of 0
                            int rowef2 = mySQLconnection.Execute(query2, parameters2, transaction);

                            // Insert into userwords table
                            string query3 = "INSERT INTO userwords (userId, wordId, userCourse_id) VALUES (@userId, @wordId, @courseId)";
                            var parameters3 = new DynamicParameters();
                            parameters3.Add("@userId", user_id);
                            parameters3.Add("@wordId", id);
                            parameters3.Add("@courseId", course_id);
                            int rowef3 = mySQLconnection.Execute(query3, parameters3, transaction);

                            if (rowefe > 0 && rowef2 > 0 && rowef3 > 0)
                            {
                                transaction.Commit();
                                return Ok("Add success");
                            }
                            else
                            {
                                transaction.Rollback();
                                return BadRequest("Something went wrong");
                            }
                        }
                        catch (MySqlException mysqlexception)
                        {
                            transaction.Rollback();
                            if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, "e003");
                            }
                            return StatusCode(StatusCodes.Status400BadRequest, "e001");
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return StatusCode(StatusCodes.Status400BadRequest, "e001");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult getAllCourse()
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "select * from courses";
                var connectDB = mySQLconnection.Query(query);
                if (connectDB != null)
                {
                    return Ok(connectDB);
                }
                else
                {

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
        [Route("export/{courseId}")]
        public IActionResult ExportWordListToExcel(string courseId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = @"
            SELECT 
                w.wordHiragana,
                w.wordMean,
                w.wordKanji,
                w.Example
            FROM 
                coursedetail cd
            INNER JOIN 
                words w ON cd.wordId = w.wordId
            WHERE 
                cd.courseId = @CourseId";

                    var wordList = mySQLconnection.Query<JLearning.Controllers.Word>(query, new { CourseId = courseId });

                    if (wordList != null && wordList.Any())
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("WordList");
                            var currentRow = 1;

                            // Add headers
                            worksheet.Cell(currentRow, 2).Value = "Hiragana";
                            worksheet.Cell(currentRow, 3).Value = "Nghĩa";
                            worksheet.Cell(currentRow, 4).Value = "Kanji";
                            worksheet.Cell(currentRow, 5).Value = "Ví dụ";

                            // Apply styling to headers
                            var headerRange = worksheet.Range(1, 2, 1, 5);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Add word list data
                            foreach (var word in wordList)
                            {
                                currentRow++;
                                worksheet.Cell(currentRow, 2).Value = word.wordHiragana;
                                worksheet.Cell(currentRow, 3).Value = word.wordMean;
                                worksheet.Cell(currentRow, 4).Value = word.wordKanji;
                                worksheet.Cell(currentRow, 5).Value = word.Example;
                            }

                            // Adjust columns width
                            worksheet.Columns().AdjustToContents();

                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                string excelName = $"Danh_sach_tu-{courseId}.xlsx";
                                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                            }
                        }
                    }
                    else
                    {
                        return NotFound("No words found for the given course.");
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
        [HttpPut]
        [Route("course/{course_id}")]
        public IActionResult UpdateCourse(string course_id, [FromBody] CoursUpdateDto updateModel)
        {
            try
            {
                // Parse the category string into a list of strings
                var categoryList = ParseCategoryString(updateModel.Category);

                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE courses SET courseName = @courseName, level = @level, category = @category, updatedAt = @updatedAt WHERE courseId = @courseId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@courseName", updateModel.CourseName);
                    parameters.Add("@level", updateModel.Level);
                    parameters.Add("@category", Newtonsoft.Json.JsonConvert.SerializeObject(categoryList));
                    parameters.Add("@updatedAt", DateTime.Now);
                    mySQLconnection.Open();
                    int rowsAffected = mySQLconnection.Execute(query, parameters);
                    Console.WriteLine("Rows affected: " + rowsAffected);
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                Console.WriteLine("MySQL exception: " + mysqlexception);

                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "e003" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
        }

        private List<string> ParseCategoryString(string categoryString)
        {
            if (string.IsNullOrEmpty(categoryString))
            {
                return new List<string>();
            }

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(categoryString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing category string: " + ex);
                return new List<string>();
            }
        }
        [HttpPut]
        [Route("learnedAt/{user_id}/{course_id}")]
        public IActionResult UpdateLearnedAt(string user_id, string course_id)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE usercourses SET learnedAt = NOW() WHERE courseId = @courseId AND userId = @userId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@userId", user_id);

                    mySQLconnection.Open(); // Ensure the connection is opened before executing the query
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    // Log the number of rows affected by the SQL update operation
                    Console.WriteLine("Rows affected:", rowsAffected);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                // Log MySQL exception details
                Console.WriteLine("MySQL exception:", mysqlexception);

                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "e003" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine("Exception:", ex);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }

        }

        [HttpPut("update-course-note/{user_id}/{course_id}/{course_note}")]
        public IActionResult UpdateCourseNote(string user_id, string course_id, int course_note)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string query = "UPDATE usercourses SET courseNote = @courseNote WHERE courseId = @courseId AND userId = @userId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@courseId", course_id);
                    parameters.Add("@userId", user_id);
                    parameters.Add("@courseNote", course_note);

                    mySQLconnection.Open();
                    int rowsAffected = mySQLconnection.Execute(query, parameters);

                    Console.WriteLine("Rows affected:", rowsAffected);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Update successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Update failed" });
                    }
                }
            }
            catch (MySqlException mysqlexception)
            {
                Console.WriteLine("MySQL exception:", mysqlexception);

                if (mysqlexception.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "e003" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:", ex);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "e001" });
            }
        }

        [HttpGet]
        [Route("courseRank/{courseId}")]
        public IActionResult GetCourseRank(string courseId)
        {
            try
            {
                using (var mySQLconnection = new MySqlConnection(DatebaseSource.name))
                {
                    string getCourseRankQuery = @"
            SELECT uc.courseId, uc.userId, uc.coursePoint, u.userName, u.usercategory, u.ImgUrl,u.userLevel
            FROM usercourses uc
            JOIN users u ON uc.userId = u.userId
            WHERE uc.courseId = @CourseId
            ORDER BY CAST(uc.coursePoint AS UNSIGNED) DESC";

                    var courseRankings = mySQLconnection.Query<dynamic>(getCourseRankQuery, new { CourseId = courseId }).ToList();

                    var result = courseRankings.Select(rank => new
                    {
                        CourseId = rank.courseId,
                        UserId = rank.userId,
                        CoursePoint = rank.coursePoint,
                        UserName = rank.userName,
                        UserLevel = rank.userLevel,
                        UserCategory = string.IsNullOrEmpty(rank.usercategory) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(rank.usercategory),
                        ImgUrl = rank.ImgUrl
                    }).ToList();

                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No users found for the given course.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

    }
}
