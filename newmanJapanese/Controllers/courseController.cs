using Dapper;
using JLearning.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace newmanJapanese.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class courseController : Controller
    {
        


        [HttpGet]
        [Route("{user_Id}")]
      //  [Authorize]

        public IActionResult Getcourse(string user_Id)
        {
            try
            {

                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getCourse = "select  courses.courseId,courseName,level,category from courses,usercourses,coursedetail where courses.courseId =usercourses.courseId AND coursedetail.courseId=courses.courseId AND userId='"+ user_Id +"' group by courses.courseId";
                IEnumerable<Course> connectDB = mySQLconnection.Query<Course>(getCourse);

                if (connectDB.First<Course>() != null)
                {
                    return Ok(connectDB);
                }
                else
                {
                    return BadRequest("Wrong something");
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

        public IActionResult createCourse(string user_Id, [FromBody] CourseCreate courseItem)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string id = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
               
                string name = courseItem.name;
                string level = courseItem.level;
                string category = courseItem.category;
                var query = "Insert into courses (courseId,category,level,courseName) value (@courseId,@category,@level,@courseName)";
                var parameters = new DynamicParameters();
                parameters.Add("@courseId", id);
                parameters.Add("@category", category);
                parameters.Add("@level", level);
                parameters.Add("@courseName", name);
                int rowefec = mySQLconnection.Execute(query,parameters);
                var query2 = "Insert into usercourses (courseId,userId) value (@courseId,@userId)";
                var parameters2 = new DynamicParameters();
                parameters2.Add("@courseId", id);
                parameters2.Add("@userId", user_Id);
                //parameters2.Add("@totalLearned", 0);
                //parameters2.Add("@courseName", DateTime.Now);
                int rowefec2 = mySQLconnection.Execute(query2,parameters2);
                if (rowefec > 0 && rowefec2 > 0)
                {
                    return Ok("Create course success");
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
        [Route("{course_Id}")]

        public IActionResult deleteCourse(string course_Id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "Delete from courses where courseId='" + course_Id + "'";
                int rowefec = mySQLconnection.Execute(query);
                if (rowefec > 0)
                {
                    return Ok("Delete success");
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
        [Route("join/{course_Id}")]

        public IActionResult getCourseinfor(string course_Id)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string query = "select words.wordId,wordStatus,wordHiragana,wordMean,wordKanji,Example from courses,words,coursedetail where courses.courseId=coursedetail.courseId AND words.wordId=coursedetail.wordId AND courses.courseId='" + course_Id + "' group by words.wordId";
                IEnumerable<CourseInfor> connectDB = mySQLconnection.Query<CourseInfor>(query);
                if (connectDB.First<CourseInfor>() != null)
                {
                    return Ok(connectDB);
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
        [Route("create/{course_id}")]
        public IActionResult createWord(string course_id, [FromBody] Word word)
        {
            try
            {
                var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string id = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string query = "insert into words (wordId,wordStatus,wordMean,wordHiragana,example) value (@wordId,@wordStatus,@wordMean,@wordHiragana,@example) ";
                var parameters = new DynamicParameters();
                parameters.Add("@wordId", id);
                parameters.Add("@wordStatus", word.wordStatus);
                parameters.Add("@wordMean", word.wordMeaning);
                parameters.Add("@wordHiragana", word.wordHiragana);
                parameters.Add("@example", word.Example);
                int rowefe = mySQLconnection.Execute(query, parameters);
                string query2 = "insert into coursedetail (courseId,wordId) value (@courseId,@wordId) ";
                var parameters2 = new DynamicParameters();
                parameters2.Add("@wordId", id);
                parameters2.Add("@courseId", course_id);
                int rowef2 = mySQLconnection.Execute(query2, parameters2);
                if (rowef2 > 0 && rowefe > 0)
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
