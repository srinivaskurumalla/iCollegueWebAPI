namespace iCollegueWebAPI.DTO
{
    public class KnowledgeBaseDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; } // For handling file upload
        public string FilePath { get; set; } // To store the file path or content
                                             // Other properties if needed
    }
}
