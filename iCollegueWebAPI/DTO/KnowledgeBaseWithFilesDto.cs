using iCollegueWebAPI.Models;

namespace iCollegueWebAPI.DTO
{

    public class KnowledgeBaseWithFilesDto
    {
      
       // public TblKnowledgeBase? KnowledgeBase { get; set; }
        public KnowledgeBaseDto? KnowledgeBaseDto { get; set; }

       
        public List<FileTable> FileTables { get; set; }
    }

}
