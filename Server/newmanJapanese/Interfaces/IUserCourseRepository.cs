using JLearning.Dto;
using JLearning.Models;

namespace JLearning.Interfaces
{
    public interface IUserCourseRepository
    {
        Task<int> UpdateUserProgressAsync(string userId, string courseId, string UserProgress);
        Task<int> UpdateCurrentWordAsync(string userId, string courseId, string CurrentWord);
        Task<UserCourseDto> GetUserCourseAsync(String  CourseId, String UserId);
    }
}
