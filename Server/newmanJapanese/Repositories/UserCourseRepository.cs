using MySqlConnector;
using Dapper;
using System.Threading.Tasks;
using JLearning.Models;
using JLearning.Repositories;
using JLearning.Interfaces;
using JLearning.Data;
using JLearning.Dto;
using Microsoft.EntityFrameworkCore;

namespace JLearning.Repositories
{
    public class UserCourseRepository
    {
        public class UserCoursesRepository : IUserCourseRepository
        {
            private readonly WebContext _context;
            private readonly UserCourseDto  _UseCourseDto;
            public UserCoursesRepository(WebContext context)
            {
                _context = context; 
            }

            public async Task<int> UpdateUserProgressAsync(string userId, string courseId, string UserProgress)
            {
                var existingUserCourse = await _context.UserCourse
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

                if (existingUserCourse != null)
                {
                    existingUserCourse.UserProgress = UserProgress;
                    return await _context.SaveChangesAsync();
                }

                return 0; 
            }

            public async Task<int> UpdateCurrentWordAsync(string userId, string courseId , string CurrentWord)
            {
                var existingUserCourse = await _context.UserCourse
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

                if (existingUserCourse != null)
                {
                    existingUserCourse.CurrentWord = CurrentWord;
                    return await _context.SaveChangesAsync();
                }

                return 0; 
            }
            public async Task<UserCourseDto> GetUserCourseAsync(string userId, string courseId)
            {
                var userCourse = await _context.UserCourse
                    .Where(uc => uc.UserId == userId && uc.CourseId == courseId)
                    .Select(uc => new UserCourseDto
                    {
                        UserProgress = uc.UserProgress,
                        CurrentWord = uc.CurrentWord
                    })
                    .FirstOrDefaultAsync();

                return userCourse;
            }
        }
    }
    }

