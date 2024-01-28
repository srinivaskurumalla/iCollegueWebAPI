namespace iCollegueWebAPI.Interfaces
{
    public interface IKnowledgeBaseWithFilesDto<T> where T : class
    {
        Task<T?> GetQueryAndFilesById(int id);
        Task<IEnumerable<T?>> GetAllQueriesAndFiles();

    }
}
