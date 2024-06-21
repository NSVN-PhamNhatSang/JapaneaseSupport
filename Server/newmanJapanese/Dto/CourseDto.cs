namespace JLearning.Dto
{
    public class CourseDto
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public double? Rating { get; set; }
        public string Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatorId { get; set; }
        public int Progress { get; set; }
        public int CurrentWord { get; set; }
        public int CoursePoint { get; set; }
        public double CoureNote { get; set; }
        public int TotalWord { get; set; }
        public int Level { get; set; }
    }
}
