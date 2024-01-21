using System;
using System.Collections.Generic;

namespace iCollegueWebAPI.Models
{
    public partial class FileTable
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public byte[]? FileContent { get; set; }
        public int? QuestionId { get; set; }

        public virtual TblKnowledgeBase? Question { get; set; }
    }
}
