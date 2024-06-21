using System.Threading.Tasks;
using JLearning.Models;
using JLearning.Interfaces;
using JLearning.Repositories;
using static JLearning.Repositories.UserCourseRepository;
using JLearning.Dto;

namespace JLearning.Services
{
    public interface IUserCoursesService
    {
        Task<int> UpdateUserProgressAsync(string UserId, string CourseId, string CurrentWord);
        Task<int> UpdateCurrentWordAsync(string UserId, string CourseId, string CurrentWord);
        Task<UserCourseDto> GetUserCourseAsync(string UserId, string CourseId);
    }
    public class UserCoursesService : IUserCoursesService
    {
        private readonly IUserCourseRepository _userCoursesRepository;

        public UserCoursesService(IUserCourseRepository userCoursesRepository)
        {
            _userCoursesRepository = userCoursesRepository;
        }

        public async Task<int> UpdateUserProgressAsync(string UserId, string CourseId, string Progress)
        {
            var result = await _userCoursesRepository.UpdateUserProgressAsync( UserId, CourseId, Progress);
            return result;
        }

        public async Task<int> UpdateCurrentWordAsync(string UserId, string CourseId, string CurrentWord)
        {
            var result = await _userCoursesRepository.UpdateCurrentWordAsync(UserId,CourseId,CurrentWord);
            return result;
        }
        public async Task<UserCourseDto> GetUserCourseAsync(string UserId, string CourseId)
        {
            var result = await _userCoursesRepository.GetUserCourseAsync(UserId,CourseId);
            return result;
        }
    }
}
