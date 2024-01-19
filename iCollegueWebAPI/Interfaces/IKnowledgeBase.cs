namespace iCollegueWebAPI.Interfaces
{
    public interface IKnowledgeBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> GetUserById(int id);
    }
}
