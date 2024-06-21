namespace newmanJapanese.Controllers
{
public class Course
{
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public double? Rating { get; set; }
    public List<string> Category { get; set; } 
    public DateTime? CreatedAt { get; set; }
    public string CreatorId { get; set; }
    public int Progress { get; set; }
    public int CurrentWord { get; set; }
    public int CoursePoint { get; set; }
    public int Level {  get; set; }
    public int TotalWord { get; set; }
        public double? CourseNote { get; set; }
        public string CreatorName { get; set; }
    public string CreatorImgUrl { get; set; } 
    }

}
