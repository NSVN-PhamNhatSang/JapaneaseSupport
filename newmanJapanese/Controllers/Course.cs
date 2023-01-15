namespace newmanJapanese.Controllers
{
    public class Course
    {
        public Guid courseId { get; set; }
        public string name { get; set; }
        public string category { set; get; }
        public int level { get; set; }
        public double rating { get; set; }
        public  int totalUserLearned { get; set; }
        public int totalWord { get; set; }
       public  int totalWordLearned { set; get; }
    }
}
