using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
namespace newmanJapanese.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class courseController : Controller
    {
        [HttpPost]
        [Route("[action]")]
        public IActionResult register(string userId,string courseId)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=learning;Uid=root;Pwd=yasuo1234gg";
                var mySQLconnection = new MySqlConnection(connectionString);
              
                string JoinCourse = "insert into usercourses (userId,courseId,totalLearned,lastLearn) values (@userId,@courseId,@totalLearned,@lastLearn)";
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId);
                parameters.Add("@courseId", courseId);
                parameters.Add("@totalLearned",  1);
                parameters.Add("@lastLearn", DateTime.Now);
                var rowNumbereffect = mySQLconnection.Execute(JoinCourse,parameters);
                if (rowNumbereffect>0)
                {
                    return Ok("Join course success");
                }
                else
                {
                    return BadRequest("You get some wrong");
                }
                return Ok();
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
        [Route("{userId}")]
        public IActionResult Getcourse(string userId)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3306;Database=learning;Uid=root;Pwd=yasuo1234gg";
                var mySQLconnection = new MySqlConnection(connectionString);
                string getCourse = "select courses.courseId as courseId,courses.name as name,category,level,rating,totalUserLearned from courses,usercourses,users where courses.courseId =usercourses.courseId AND users.userId=usercourses.userId AND users.userId='" + userId + "'";
                IEnumerable<Course> connectDB = mySQLconnection.Query<Course>(getCourse);
                foreach (var tempI in connectDB)
                {   
                    string getTotalWord = "select count(words.wordId) as totalWord from words,courses where words.courseId=courses.courseId AND courses.courseId='" + tempI.courseId + "'";
                    tempI.totalWord = mySQLconnection.QueryFirstOrDefault<int>(getTotalWord);
                    string getTotalWordLearned = "SELECT COUNT(wordId) as totalWordLearned from (select userwords.wordId from userwords, words, courses where userwords.wordId = words.wordId AND words.courseId = courses.courseId AND words.courseId = '" + tempI.courseId + "' AND userId = '" + userId + "') as new";
                    tempI.totalWordLearned = mySQLconnection.QueryFirstOrDefault<int>(getTotalWordLearned);
                }
                if (connectDB.First<Course>()!=null)
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
    }
}
