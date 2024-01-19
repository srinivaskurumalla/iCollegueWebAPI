using System;
using System.Collections.Generic;

namespace iCollegueWebAPI.Models
{
    public partial class TblKnowledgeBase
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Description { get; set; }
    }
}
