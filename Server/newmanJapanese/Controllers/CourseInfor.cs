namespace JLearning.Controllers
{
    public class CourseInfor
    {
        //public Guid notewordId { get; set; }
        //public Guid notebookId { get; set; }

        public string wordId { get; set; }
        public string wordHiragana { get; set; }
        public string wordMean { get; set; }
        public string wordKanji { get; set; }
        public string Example { get; set; }
        public string wordImg { get; set;}
       // public string example { get; set; }
       public int wordNote { get; set; }
        public int? coursePoint { get; set; }

    }
}
