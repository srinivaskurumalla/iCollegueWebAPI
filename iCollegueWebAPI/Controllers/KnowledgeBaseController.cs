using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using iCollegueWebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iCollegueWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBase<TblKnowledgeBase> _knowledgeBaseRepo;

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
    }
}
