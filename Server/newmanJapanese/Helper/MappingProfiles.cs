using JLearning.Models;
using JLearning.Dto;
using AutoMapper;

namespace JLearning.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            CreateMap<UserDetail, UserDetailDto>();
            CreateMap<UserCourse, UserCourseDto>();
        }
    }
}
