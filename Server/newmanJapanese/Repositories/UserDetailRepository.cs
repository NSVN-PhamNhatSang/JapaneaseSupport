using JLearning.Data;
using JLearning.Dto;
using JLearning.Interfaces;
using JLearning.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JLearning.Repository
{
    public class UserDetailRepository : IUserDetailRepository
    {
        private readonly WebContext _context;
        private readonly UserDetailDto _userDetailDto;
        public UserDetailRepository(WebContext context) { _context = context; }
        public ICollection<UserDetail> GetAll()
        {
            return _context.UserDetail.OrderBy(user => user.userId).ToList();
        }


        public UserDetail GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public UserDetail GetUserById(string userId)
        {
            return _context.UserDetail.FirstOrDefault(i => i.userId == userId);
        }

        public UserDetail GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public UserDetail UpdateInfo(string userId, UserDetail userdetail)
       {
            var existingUser = _context.UserDetail.Find(userId);

            if (existingUser != null)
            {
                // Cập nhật thông tin của existingUser từ userdetail
                existingUser.Email = userdetail.Email;
                existingUser.Address = userdetail.Address;
                existingUser.Bio = userdetail.Bio;
        

                // Lưu các thay đổi xuống cơ sở dữ liệu
                _context.SaveChanges();

                // Trả về đối tượng UserDetail đã được cập nhật
                return existingUser;
            }
            else
            {
                // Xử lý trường hợp userId không tồn tại
                // (ví dụ: ném một ngoại lệ hoặc trả về null)
                throw new ArgumentException("User not found");
            }
        }

        public UserDetail UpdateLevel(string level)
        {
            throw new NotImplementedException();
        }

        public UserDetail UpdateUserCategory(string category)
        {
            throw new NotImplementedException();
        }

    }
}
