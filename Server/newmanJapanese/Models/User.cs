using System.ComponentModel.DataAnnotations;

namespace JLearning.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public int? UserLevel { get; set; }
        public string? UserCategory { get; set; } // JSON stored as string
        public int? Roll { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserPoint { get; set; }
    }
}
