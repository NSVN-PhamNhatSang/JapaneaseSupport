namespace JLearning.Controllers
{
    public class NoteBook
    {
        public Guid notebookId { get; set; }
        public Guid userId { get; set; }
        public string name { get; set; }
        public int totalWord { get; set; }
        public DateTime createAt {get; set; }
        public DateTime lastLearnAt {get; set; }

    }
}
