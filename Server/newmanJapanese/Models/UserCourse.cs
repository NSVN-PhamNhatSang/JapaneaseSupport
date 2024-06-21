using System.ComponentModel.DataAnnotations;
namespace JLearning.Models
{
    public class UserCourse
    {
        [Key]
        public string UserId { get; set; }
        public string CourseId { get; set; }
        public DateTime? LearnedAt { get; set; }
        public string CourseStatus { get; set; }
        public double? CourseNote { get; set; }
        public string UserProgress { get; set; } = "0";
        public string CurrentWord { get; set; } = "0";
        public string CoursePoint { get; set; }

    }
}
