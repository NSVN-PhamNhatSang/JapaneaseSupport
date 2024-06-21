using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JLearning.Models;
using JLearning.Services;
using JLearning.Dto;

namespace JLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCoursesController : ControllerBase
    {
        private readonly IUserCoursesService _userCoursesService;

        public UserCoursesController(IUserCoursesService userCoursesService)
        {
            _userCoursesService = userCoursesService;
        }

        [HttpPut("updateUserProgress")]
        public async Task<IActionResult> UpdateUserProgress(string UserId, string CourseId, string Progress)
        {
            var result = await _userCoursesService.UpdateUserProgressAsync(UserId, CourseId,Progress);
            if (result > 0)
            {
                return Ok("User progress updated successfully.");
            }
            return NotFound("User course not found.");
        }

        [HttpPut("updateCurrentWord")]
        public async Task<IActionResult> UpdateCurrentWord(string UserId, string CourseId, string CurrentWord)
        {
            var result = await _userCoursesService.UpdateCurrentWordAsync( UserId,  CourseId, CurrentWord);
            if (result > 0)
            {
                return Ok("Current word updated successfully.");
            }
            return NotFound("User course not found.");
        }
        [HttpGet("{userId}/{courseId}")]
        public async Task<ActionResult<UserCourseDto>> GetUserCourse(string userId, string courseId)
        {
            var userCourse = await _userCoursesService.GetUserCourseAsync(userId, courseId);

            if (userCourse == null)
            {
                return NotFound();
            }

            return Ok(userCourse);
        }
    }
}
