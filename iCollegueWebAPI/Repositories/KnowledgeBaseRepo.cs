using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iCollegueWebAPI.Repositories
{
    public class KnowledgeBaseRepo : IKnowledgeBase<TblKnowledgeBase>
    {
        private readonly iColleagueContext _dbContext;

        public KnowledgeBaseRepo(iColleagueContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TblKnowledgeBase>> GetAll()
        {
           return await _dbContext.TblKnowledgeBases.ToListAsync();
        }

        public async Task<TblKnowledgeBase?> GetUserById(int id)
        {
           var result =  await _dbContext.TblKnowledgeBases.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
    }
}
