using iCollegueWebAPI.Interfaces;
using iCollegueWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iCollegueWebAPI.Repositories
{
    public class KnowledgeBaseRepo : IKnowledgeBase<TblKnowledgeBase>, IKnowledgeBaseWithFilesDto<KnowledgeBaseDto>
    {
        private readonly iColleagueContext _dbContext;

        public KnowledgeBaseRepo(iColleagueContext dbContext)
        {
            _dbContext = dbContext;
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
        /*           public async Task<dynamic?> GetQueryAndFilesById(int id)
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
        */
        public async Task<KnowledgeBaseDto?> GetQueryAndFilesById(int id)
        {

            // var result = await _dbContext.TblKnowledgeBases.Include(x => x.FileTables).FirstOrDefaultAsync(x => x.Id == id);

            var result = await _dbContext.TblKnowledgeBases
                                                     .Include(x => x.FileTables)
                                                     .Select(kb => new KnowledgeBaseDto
                                                     {
                                                         Id = kb.Id,
                                                         Question = kb.Question,
                                                         Answer = kb.Answer,
                                                         Description = kb.Description,
                                                         FileTables = kb.FileTables.Select(ft => new FileTable
                                                         {
                                                             FileId = ft.FileId,
                                                             FileName = ft.FileName,
                                                             FilePath = ft.FilePath,
                                                             FileContent = ft.FileContent,
                                                             // Map other properties as needed
                                                         }).ToList()
                                                     })
                                                     .FirstOrDefaultAsync(x => x.Id == id);

            // Filter out nested FileTables in the question property
           // result?.FileTables.ForEach(ft => ft.Question.FileTables = null);

            if (result == null)
            {
                return null;
            }
            else
            {
                /* var kbf = new KnowledgeBaseDto
                 {
                     KnowledgeBaseDto = result,
                     FileTables = result.FileTables.ToList()
                 };
                 return kbf;*/
                return result;
            }

            
        }

        public async Task<IEnumerable<KnowledgeBaseDto?>> GetAllQueriesAndFiles()
        {
            return await _dbContext.TblKnowledgeBases.Include(x => x.FileTables)
                                                      .Select(kb => new KnowledgeBaseDto
                                                      {
                                                          Id = kb.Id,
                                                          Question = kb.Question,
                                                          Answer = kb.Answer,
                                                          Description = kb.Description,
                                                          FileTables = kb.FileTables.Select(ft => new FileTable
                                                          {
                                                              FileId = ft.FileId,
                                                              FileName = ft.FileName,
                                                              FilePath = ft.FilePath,
                                                              FileContent = ft.FileContent,
                                                          }).ToList(),
                                                      }).ToListAsync();

        }
    }
}
