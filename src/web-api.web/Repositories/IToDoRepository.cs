using web_api.web.Models;

namespace web_api.web.Repositories
{
    public interface IToDoRepository
    {
        Task<IReadOnlyList<ToDo>> GetAllAsync();
        Task<ToDo?> GetByIdAsync(int id);
        Task<ToDo> CreateAsync(ToDo toDo);
        Task<bool> UpdateAsync(ToDo toDo);
        Task<bool> DeleteAsync(int id);
    }
}
