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

        public KnowledgeBaseRepo()
        {
        }

        public async Task<int> Create(TblKnowledgeBase obj)
        {
            if (obj == null)
            {
                return -1;
            }
            else
            {
                // #To-do  #need to check whethe query is already available in db
                // var queryExists = _dbContext.TblKnowledgeBases.Any(k  => k.Id == obj.Id);
                _dbContext.Add(obj);
                await _dbContext.SaveChangesAsync();
                return obj.Id;
            }
        }

        public async Task<IEnumerable<TblKnowledgeBase>> GetAll()
        {
            return await _dbContext.TblKnowledgeBases.ToListAsync();
        }

        public async Task<TblKnowledgeBase?> GetQueryById(int id)
        {
            var result = await _dbContext.TblKnowledgeBases.FirstOrDefaultAsync(x => x.Id == id);

           /* var result = _dbContext.TblKnowledgeBases
                                .Include(kb => kb.FileTables) // Include the related files
                                .FirstOrDefault(kb => kb.Id == id);
*/
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
           public async Task<dynamic?> GetQueryAndFilesById(int id)
        {
            //  var result = await _dbContext.TblKnowledgeBases.FirstOrDefaultAsync(x => x.Id == id);

            var result = _dbContext.TblKnowledgeBases
                                .Include(kb => kb.FileTables) // Include the related files
                                .FirstOrDefault(kb => kb.Id == id);

            if (result == null)
            {
                return null;
            }
            else
            {
                var knowledgeBaseWithFiles = new KnowledgeBaseWithFiles
                {
                    KnowledgeBase = result,
                    FileTables = result.FileTables?.ToList() ?? new List<FileTable>() // Ensure the collection is materialized

                };
                return knowledgeBaseWithFiles;
            }
        }


    }
}
