using iCollegueWebAPI.Models;

namespace iCollegueWebAPI.Interfaces
{
    public interface IKnowledgeBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> GetQueryById(int id);
        Task<int> Create(T obj);
      //  Task<int> CreateFile(T obj);
    }
  
}
