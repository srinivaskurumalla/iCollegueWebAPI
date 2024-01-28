namespace iCollegueWebAPI.Models
{
    public partial class TblKnowledgeBase
    {
        public TblKnowledgeBase()
        {
            FileTables = new HashSet<FileTable>();
        }

        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<FileTable> FileTables { get; set; }
    }

    public class KnowledgeBaseDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public List<FileTable> FileTables { get; set; }
    }
}
