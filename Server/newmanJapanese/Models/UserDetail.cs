using System.ComponentModel.DataAnnotations;
namespace JLearning.Models
{
    public class UserDetail
    {
        [Key]
        public string userId { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        //public ICollection<User> userCategories { get; set; }
      
    }

}
