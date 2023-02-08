using System.ComponentModel.DataAnnotations;

namespace GoodFood.api.Models
{
    public class UserLogins
    {
        [Required]
        public string userName
        {
            get;
            set;
        }
        [Required]
        public string userPassword
        {
            get;
            set;
        }
        public UserLogins() { }
    }
}
