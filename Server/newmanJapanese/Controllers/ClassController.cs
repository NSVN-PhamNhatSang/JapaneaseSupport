using CloudinaryDotNet;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        [HttpPost]
        [Route("createClass/{teacher_Id}")]
        public async Task<IActionResult> CreateClass(string teacher_Id, [FromBody] CreateClass newClass)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Generate random classId
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string classId = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string insertClass = @"INSERT INTO class (classId, className, teacherId, courseId, startDate, endDate, description,school,level) 
                                       VALUES (@classId, @className, @teacherId, @courseId, @startDate, @endDate,@description,@school,@level)";
                var parameter = new
                {
                    classId,
                    newClass.className,
                    teacherId = teacher_Id,
                    courseId = (string?)null, // Assuming courseId can be null
                    newClass.startDate,
                    newClass.endDate,
                    newClass.description,
                    newClass.school,
                    newClass.level
                };
                await mySQLconnection.ExecuteAsync(insertClass, parameter);
                return Ok(new { message = "Class created successfully", classId });
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }
        [HttpGet("enrollmentRequests/{classId}")]
        public async Task<IActionResult> GetEnrollmentRequests(string classId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getRequests = @"
            SELECT u.*,cd.classDetailId, cd.studentId, u.username AS studentName, cd.enrollmentStatus
            FROM class_details cd
            INNER JOIN users u ON cd.studentId = u.userId
            WHERE cd.classId = @ClassId AND cd.enrollmentStatus = 'pending'
        ";
                var requests = await mySQLconnection.QueryAsync<EnrollmentRequest>(getRequests, new { ClassId = classId });

                if (requests.Any())
                {
                    return Ok(requests);
                }
                else
                {
                    return NotFound("No enrollment requests found for the given class.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }
        [HttpGet("classDetails/{classId}")]
        public async Task<IActionResult> GetClassDetails(string classId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                string getClassDetails = @"
            SELECT 
                *
            FROM class cl
            INNER JOIN users u ON cl.teacherId = u.userId
            INNER JOIN class_courses cc ON cl.classId = cc.classId
            INNER JOIN courses c ON cc.courseId = c.courseId
            WHERE cl.classId = @ClassId
            LIMIT 1
        ";

                var classDetail = await mySQLconnection.QueryFirstOrDefaultAsync<dynamic>(getClassDetails, new { ClassId = classId });

                if (classDetail != null)
                {
                    return Ok(classDetail);
                }
                else
                {
                    return NotFound("No details found for the given class.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }



        [HttpGet("enrolled/{student_id}")]
        public async Task<IActionResult> GetEnrolledClasses(string student_id)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getClasses = @"
            SELECT c.*, u.*
            FROM class c
            INNER JOIN class_details g ON c.classId = g.classId
            INNER JOIN users u ON g.studentId = u.userId
            WHERE g.studentId = @StudentId
        ";
                var result = await mySQLconnection.QueryAsync(getClasses, new { StudentId = student_id });

                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No classes found for the given student.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }


        [HttpGet("created/{user_id}")]
        public async Task<IActionResult> getCreatedClass(string user_id)
        {
            try {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string getClass = @"
                    SELECT *
                    FROM class
                    WHERE teacherId = @TeacherId
                ";
                var classes = await mySQLconnection.QueryAsync<Class>(getClass, new { TeacherId = user_id });

                var result = classes.Select(c => new
                {
                    c.classId,
                    c.className,
                    c.description,
                    c.school,
                    c.teacherId,
                    c.courseId,
                    c.startDate,
                    c.endDate,
                    c.memberCount,
                    c.courseCount,
                    c.level
                }).ToList();

                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No classes found for the given user.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }



        [HttpDelete("deleteClass/{classId}")]
        public async Task<IActionResult> DeleteClass(string classId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Ensure all related rows in class_details are deleted first
                string deleteClassDetails = "DELETE FROM class_details WHERE classId = @classId";
                var parameterDetails = new { classId };
                await mySQLconnection.ExecuteAsync(deleteClassDetails, parameterDetails);

                // Now delete the class itself
                string deleteClass = "DELETE FROM class WHERE classId = @classId";
                var parameter = new { classId };
                await mySQLconnection.ExecuteAsync(deleteClass, parameter);

                return Ok(new { message = "Class and related details deleted successfully" });
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }


        [HttpPost("addMember/{classId}/{studentId}")]
        public async Task<IActionResult> AddMember(string classId, string studentId, [FromBody] ClassMember member)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Generate random classDetailId
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string classDetailId = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string insertMember = @"INSERT INTO class_details (classDetailId, classId, studentId, grade, gradeDate, enrollmentStatus) 
                                        VALUES (@classDetailId, @classId, @studentId, @grade, @gradeDate, @enrollmentStatus)";
                var parameter = new
                {
                    classDetailId,
                    classId,
                    studentId,
                    member.grade,
                    member.gradeDate,
                    enrollmentStatus = "Inactive" // Default status
                };
                await mySQLconnection.ExecuteAsync(insertMember, parameter);
                return Ok(new { message = "Member added successfully" });
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }


        [HttpDelete("removeMember/{classId}/{studentId}")]
        public async Task<IActionResult> RemoveMember(string classId, string studentId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                string deleteMember = "DELETE FROM class_details WHERE classId = @classId AND studentId = @studentId";
                var parameter = new { classId, studentId };
                await mySQLconnection.ExecuteAsync(deleteMember, parameter);
                return Ok(new { message = "Member removed successfully" });
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }
        [HttpPost("{classId}/addCourse/{courseId}")]
        public async Task<IActionResult> AddCourseToClass(string classId, string courseId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Check if the course already exists in the class
                string checkCourseInClass = @"SELECT COUNT(*) FROM class_courses WHERE classId = @classId AND courseId = @courseId";
                var checkParameters = new { classId, courseId };
                var existingCount = await mySQLconnection.ExecuteScalarAsync<int>(checkCourseInClass, checkParameters);

                if (existingCount == 0)
                {
                    // Generate random classCourseId
                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    int length = 10;
                    string classCourseId = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                    // Begin a transaction
                    using var transaction = await mySQLconnection.BeginTransactionAsync();

                    try
                    {
                        // Insert into class_courses
                        string insertClassCourse = @"INSERT INTO class_courses (classCourseId, classId, courseId) 
                                             VALUES (@classCourseId, @classId, @courseId)";
                        var classCourseParameters = new
                        {
                            classCourseId,
                            classId,
                            courseId
                        };
                        await mySQLconnection.ExecuteAsync(insertClassCourse, classCourseParameters, transaction);

                        // Update courseCount in class
                        string updateClassCourseCount = @"UPDATE class SET courseCount = courseCount + 1 WHERE classId = @classId";
                        var classParameters = new { classId };
                        await mySQLconnection.ExecuteAsync(updateClassCourseCount, classParameters, transaction);

                        // Commit the transaction
                        await transaction.CommitAsync();

                        return Ok(new { message = "Course added to class successfully", classCourseId });
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                else
                {
                    // If the course already exists in the class, return a conflict response
                    return Conflict(new { error = "Course already exists in the class" });
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }




        [HttpDelete("{classId}/removeCourse/{courseId}")]
            public async Task<IActionResult> RemoveCourseFromClass(string classId, string courseId)
            {
                try
                {
                    using var mySQLconnection = new MySqlConnection(DatebaseSource.name);
                    string deleteClassCourse = "DELETE FROM class_courses WHERE classId = @classId AND courseId = @courseId";
                    var parameter = new { classId, courseId };
                    await mySQLconnection.ExecuteAsync(deleteClassCourse, parameter);
                    return Ok(new { message = "Course removed from class successfully" });
                }
                catch (MySqlException mysqlexception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
                }
            }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClass([FromQuery] string className)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                string searchClassQuery = @"
                    SELECT *
                    FROM class c
                    JOIN users u ON c.teacherId = u.userId
                    WHERE c.className LIKE @className";
                var parameter = new { className = "%" + className + "%" };

                var classes = await mySQLconnection.QueryAsync<dynamic>(searchClassQuery, parameter);

                return Ok(classes);
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }

        [HttpPost("requestEnrollment/{classId}/{studentId}")]
        public async Task<IActionResult> RequestEnrollment(string classId, string studentId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Giả sử bạn có một phương thức để lấy teacherId từ classId
                string getTeacherIdQuery = "SELECT teacherId FROM class WHERE classId = @classId";
                var teacherId = await mySQLconnection.QueryFirstOrDefaultAsync<string>(getTeacherIdQuery, new { classId });

                // Kiểm tra nếu studentId trùng với teacherId
                if (studentId == teacherId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { error = "Teachers cannot request enrollment in their own course." });
                }

                // Generate random classDetailId
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int length = 10;
                string classDetailId = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                string insertRequest = @"INSERT INTO class_details (classDetailId, classId, studentId, enrollmentStatus) 
                                 VALUES (@classDetailId, @classId, @studentId, 'pending')";
                var parameter = new
                {
                    classDetailId,
                    classId,
                    studentId
                };
                await mySQLconnection.ExecuteAsync(insertRequest, parameter);
                return Ok(new { message = "Enrollment request sent successfully", classDetailId });
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }
        [HttpGet("activeStudents/{classId}")]
        public async Task<IActionResult> GetActiveStudents(string classId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                string getActiveStudents = @"
            SELECT *
            FROM users u
            INNER JOIN class_details cd ON u.userId = cd.studentId
            WHERE cd.classId = @ClassId AND cd.enrollmentStatus = 'active'
        ";

                var activeStudents = await mySQLconnection.QueryAsync<User>(getActiveStudents, new { ClassId = classId });

                if (activeStudents.Any())
                {
                    return Ok(activeStudents);
                }
                else
                {
                    return NotFound("No active students found for the given class.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }

        [HttpGet("coursesInClass/{classId}")]
        public async Task<IActionResult> GetCoursesInClass(string classId)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                string getCourses = @"
            SELECT c.courseId, c.courseName, c.rating, c.category, c.level, c.numberRating, c.createdAt, c.updatedAt, c.courseImg, c.creatorId, c.courseNote, c.totalWord
            FROM courses c
            INNER JOIN class_courses cc ON c.courseId = cc.courseId
            WHERE cc.classId = @ClassId
        ";

                var courses = await mySQLconnection.QueryAsync<dynamic>(getCourses, new { ClassId = classId });

                if (courses.Any())
                {
                    return Ok(courses);
                }
                else
                {
                    return NotFound("No courses found for the given class.");
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }

        [HttpPost("manageEnrollment/{classId}/{studentId}")]
        public async Task<IActionResult> ManageEnrollment(string classId, string studentId, [FromBody] ApprovalRequest approvalRequest)
        {
            try
            {
                using var mySQLconnection = new MySqlConnection(DatebaseSource.name);

                // Mở kết nối cơ sở dữ liệu
                await mySQLconnection.OpenAsync();

                // Begin a transaction
                using var transaction = await mySQLconnection.BeginTransactionAsync();

                try
                {
                    if (approvalRequest.enrollmentStatus == "active")
                    {
                        // Update enrollment status to 'active'
                        string updateStatus = @"UPDATE class_details 
                                SET enrollmentStatus = @enrollmentStatus 
                                WHERE classId = @classId AND studentId = @studentId";
                        var parameter = new
                        {
                            classId,
                            studentId,
                            approvalRequest.enrollmentStatus
                        };
                        await mySQLconnection.ExecuteAsync(updateStatus, parameter, transaction);

                        // Update member count in class
                        string updateMemberCount = @"UPDATE class 
                                     SET memberCount = memberCount + 1 
                                     WHERE classId = @classId";
                        await mySQLconnection.ExecuteAsync(updateMemberCount, new { classId }, transaction);
                    }
                    else if (approvalRequest.enrollmentStatus == "rejected")
                    {
                        // Delete enrollment from class_details
                        string deleteEnrollment = @"DELETE FROM class_details 
                                            WHERE classId = @classId AND studentId = @studentId";
                        var parameter = new { classId, studentId };
                        await mySQLconnection.ExecuteAsync(deleteEnrollment, parameter, transaction);
                    }
                    else
                    {
                        return BadRequest("Invalid enrollment status. Allowed values: 'active', 'rejected'");
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();

                    return Ok(new { message = "Enrollment status updated successfully" });
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (MySqlException mysqlexception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Database error. Please try again later.", details = mysqlexception.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Unknown error occurred. Please try again later.", details = ex.Message });
            }
        }





        public class ApprovalRequest
        {
            public string enrollmentStatus { get; set; } // Values: "approved" or "rejected"
        }


    }

    public class Class
    {
        public string? classId { get; set; }
        public string className { get; set; }
        public string? school { get; set; }
        public string? description { get; set; }
        public string? teacherId { get; set; }
        public string? courseId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? courseCount { get; set; }
        public int? memberCount { get; set; }
        public int? level { get; set; }  
    }
    public class CreateClass
    {
        public string className { get; set; }
        public string? school { get; set; }
        public string? description { get; set; }
        public int level { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }

    public class ClassMember
    {
        public string? classId { get; set; }
        public string? studentId { get; set; }
        public double? grade { get; set; }
        public DateTime? gradeDate { get; set; }
    }

    public class Course
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
    }
    public class EnrollmentRequest
    {
        public string userLevel { get; set; }
        public int userPoint { get;set; }
        public string classDetailId { get; set; }
        public string studentId { get; set; }
        public string studentName { get; set; }
        public string enrollmentStatus { get; set; }
        public string ImgUrl { get; set; }
    }


}
