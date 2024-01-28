using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Image = System.Drawing.Image;

namespace iCollegueWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBase<TblKnowledgeBase> _knowledgeBaseRepo;
        private readonly IKnowledgeBaseWithFilesDto<KnowledgeBaseDto> _knowledgeBaseWithFilesRepo;
        private readonly iColleagueContext _iColleagueContext;

        public KnowledgeBaseController(IKnowledgeBase<TblKnowledgeBase> knowledgeBaseRepo, IKnowledgeBaseWithFilesDto<KnowledgeBaseDto> knowledgeBaseWithFilesRepo, iColleagueContext colleagueContext)
        {
            _knowledgeBaseRepo = knowledgeBaseRepo;
            _knowledgeBaseWithFilesRepo = knowledgeBaseWithFilesRepo;
            _iColleagueContext = colleagueContext;
        }






        // private readonly IConfiguration _configuration;



        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllData()
        {
            var data = await _knowledgeBaseRepo.GetAll();
            return Ok(data);
        }
        [HttpGet("GetAllQueriesWithFiles")]
        public async Task<IActionResult> GetAllQueriesWithFiles()
        {
            var data = await _knowledgeBaseWithFilesRepo.GetAllQueriesAndFiles();
            if(data !=  null)
            {
                return Ok(data);

            }
            else { return NotFound(); }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetQueryById(int id)
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
        [HttpGet("GetQueryAndFilesById/{id}")]
        public async Task<ActionResult<KnowledgeBaseDto>> GetQueryAndFilesById(int id)
        {
            try
            {
                var result = await _knowledgeBaseWithFilesRepo.GetQueryAndFilesById(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(500, "Internal Server Error"+ex);
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
        public async Task<IActionResult> UploadFileAsync(IFormFile file,int queryId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();

                // Save fileBytes to SQL Server using Entity Framework or another data access method
                // Example:
                var newFile = new FileTable { FileContent = fileBytes, FileName = file.FileName ,QuestionId = queryId};
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
            if(contentType == "jpeg" || contentType == "png" || contentType == "jpg")
            {
                var fileContentString = fileEntity.FileContent.ToString();
                DisplayImage(fileContentString);
            }
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
        //   [HttpGet("GetImageFromBytes/{base64Image}")]
        [NonAction]
        public IActionResult DisplayImage(string base64Image)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                Image image = ByteArrayToImage(imageBytes);

                if (image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Convert Image to byte[] and send it as a response
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return File(ms.ToArray(), "image/jpeg");
                    }
                }

                // Return a placeholder image or handle the case where the conversion fails
                return File(new byte[0], "image/jpeg");
            }
            catch (FormatException)
            {
                // Handle invalid base64 string
                return BadRequest("Invalid base64 string");
            }
        }


        private static Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return Image.FromStream(stream);
            }
        }
    }
}





