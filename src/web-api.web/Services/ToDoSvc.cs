using Microsoft.EntityFrameworkCore;
using web_api.web.Data;
using web_api.web.Models;

namespace web_api.web.Services
{
    public class ToDoSvc
    {
        private readonly ToDoContext _context;
        private readonly ILogger<ToDoSvc> _logger;

        public ToDoSvc(ToDoContext context, ILogger<ToDoSvc> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IReadOnlyList<ToDo>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all ToDo items.");
            var toDos = await _context.ToDos.ToListAsync();
            return toDos.AsReadOnly();
        }

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving ToDo item with Id {Id}.", id);
            return await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ToDo> CreateAsync(ToDo toDo)
        {
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new ToDo item with Id {Id}.", toDo.Id);
            return toDo;
        }

        public async Task<bool> UpdateAsync(ToDo toDo)
        {
            var existingToDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == toDo.Id);
            if (existingToDo == null)
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for update.", toDo.Id);
                return false;
            }

            existingToDo.Title = toDo.Title;
            existingToDo.IsCompleted = toDo.IsCompleted;
            existingToDo.Description = toDo.Description;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated ToDo item with Id {Id}.", toDo.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
            if (toDo == null)
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for deletion.", id);
                return false;
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted ToDo item with Id {Id}.", id);
            return true;
        }
    }
}
