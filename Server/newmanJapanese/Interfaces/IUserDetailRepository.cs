using JLearning.Dto;
using JLearning.Models;

namespace JLearning.Interfaces
{
    public interface IUserDetailRepository
    {
        ICollection<UserDetail> GetAll();
        UserDetail GetUserById(string userId);
        UserDetail GetUserByName(string userName);
        UserDetail GetUserByEmail(string email);  
        UserDetail UpdateLevel(string  level);
        UserDetail UpdateUserCategory(string category);
        UserDetail UpdateInfo(string userId, UserDetail userdetail);

        bool Save();
    }
}
