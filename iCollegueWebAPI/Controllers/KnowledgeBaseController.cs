using iCollegueWebAPI.DTO;
using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using iCollegueWebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iCollegueWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBase<TblKnowledgeBase> _knowledgeBaseRepo;
        //private readonly IKnowledgeBaseService _knowledgeBaseService;
        //private readonly iColleagueContext _dbContext;

        public KnowledgeBaseController(IKnowledgeBase<TblKnowledgeBase> knowledgeBaseRepo)
        {
            _knowledgeBaseRepo = knowledgeBaseRepo;
        }

       

        // private readonly IConfiguration _configuration;



        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _knowledgeBaseRepo.GetAll();
            return Ok(data);
        } 
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetAllUsers(int id)
        {
            var result = await _knowledgeBaseRepo.GetQueryById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("PostQuery")]
        public async Task<IActionResult> PostQuery([FromBody] TblKnowledgeBase tblKnowledgeBase)
        {
            var queryAdded = await _knowledgeBaseRepo.Create(tblKnowledgeBase);
            if (queryAdded != -1)
            {
                return Ok(queryAdded);
            }
            else
            {
                return NotFound();
            }
        }

        /* [HttpPost("PostQuery")]
         public async Task<IActionResult> PostQuery([FromBody] KnowledgeBaseDto knowledgeBaseDto)
         {
             try
             {
                 if (knowledgeBaseDto.File != null && knowledgeBaseDto.File.Length > 0)
                 {
                     // Handle file upload here (store it, etc.)
                     // For simplicity, you can save it to a folder
                     //var filePath = "path_to_your_upload_folder/" + knowledgeBaseDto.File.FileName;
                     var filePath =  knowledgeBaseDto.File.FileName;
                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         await knowledgeBaseDto.File.CopyToAsync(stream);
                     }

                     // Now, save the record with the file path
                     knowledgeBaseDto.FilePath = filePath;
                 }

                 // Map your DTO to the entity
                 var knowledgeBase = new TblKnowledgeBase
                 {
                     Question = knowledgeBaseDto.Question,
                     Answer = knowledgeBaseDto.Answer,
                     Description = knowledgeBaseDto.Description,
                     // Other properties...
                 };

                 // Save the knowledge base record
                 var knowledgeBaseId = await _knowledgeBaseRepo.Create(knowledgeBase);

                 // Save the associated file record
                 if (!string.IsNullOrEmpty(knowledgeBaseDto.FilePath))
                 {
                     var fileTable = new FileTable
                     {
                         QuestionId = knowledgeBaseId,
                         FileName = knowledgeBaseDto.File.FileName,
                        // FileContent = knowledgeBaseDto.FilePath
                         // Other properties...
                     };

                     _dbContext.FileTables.Add(fileTable);
                     await _dbContext.SaveChangesAsync();
                 }

                 return Ok("Record and file uploaded successfully");
             }
             catch (Exception ex)
             {
                 // Handle exceptions appropriately
                 return StatusCode(500, $"Internal Server Error: {ex.Message}");
             }
         }
 */
    }
}
