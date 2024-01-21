using iCollegueWebAPI.DTO;
using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using iCollegueWebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace iCollegueWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBase<TblKnowledgeBase> _knowledgeBaseRepo;
        /*        private readonly KnowledgeBaseRepo knowledgeBaseRepo1;
        */        //private readonly IKnowledgeBaseService _knowledgeBaseService;
        private readonly iColleagueContext _iColleagueContext;

        /*  public KnowledgeBaseController(IKnowledgeBase<TblKnowledgeBase> knowledgeBaseRepo)
          {
              _knowledgeBaseRepo = knowledgeBaseRepo;
          }*/

        public KnowledgeBaseController(IKnowledgeBase<TblKnowledgeBase> knowledgeBaseRepo, iColleagueContext _iColleagueContext)
        {
            _knowledgeBaseRepo = knowledgeBaseRepo;
            this._iColleagueContext = _iColleagueContext;
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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();

                // Save fileBytes to SQL Server using Entity Framework or another data access method
                // Example:
                var newFile = new FileTable { FileContent = fileBytes, FileName = file.FileName };
                _iColleagueContext.FileTables.Add(newFile);
                await _iColleagueContext.SaveChangesAsync();
            }

            return Ok(new { message = "File uploaded successfully" });
        }

        [HttpGet("GetFileById/{fileId}")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var fileEntity = await _iColleagueContext.FileTables.FindAsync(fileId);

            if (fileEntity == null)
                return NotFound("File not found");


            // Determine content type based on file extension
            var contentType = GetContentType(fileEntity.FileName);

            // Set Content-Disposition header to suggest a filename for the downloaded file
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileEntity.FileName,
                FileNameStar = fileEntity.FileName
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            // Return the file content as a FileStreamResult with content type
            return File(fileEntity.FileContent, contentType, fileEntity.FileName);
        }
        // Assuming YourFileEntity has a 'Content' property of type byte[] to store file content
        //byte[] fileContent = fileEntity.FileContent;

        // You may need to determine the file content type and set the appropriate response headers
        // For example, if it's a PDF file:
        // Response.Headers.Add("Content-Type", "application/pdf");

        // Return the file content as a FileStreamResult
        //return File(fileContent, "application/octet-stream", fileEntity.FileName);


        [NonAction]
        private string GetContentType(string fileName)
        {
            // Map file extensions to MIME types
            var mimeTypes = new Dictionary<string, string>
        {
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            // Add more mappings as needed
        };

            // Get file extension
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            // Look up the extension in the mapping
            if (extension != null && mimeTypes.TryGetValue(extension, out var contentType))
            {
                return contentType;
            }

            // Default to binary/octet-stream if the extension is not recognized
            return "application/octet-stream";
        }
    }
}





