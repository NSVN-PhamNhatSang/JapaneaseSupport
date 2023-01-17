namespace JLearning.Controllers
{
    public class NoteWord
    {
        public Guid notewordId { get; set; }
        public Guid notebookId { get; set; }
        public string kanji { get; set; }
        public string cvword { get; set; }
        public string hirakata { get; set; }
        public string mean { get; set; }
        public string example { get; set; }
    }
}
