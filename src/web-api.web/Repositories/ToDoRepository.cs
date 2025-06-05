using Microsoft.EntityFrameworkCore;
using web_api.web.Data;
using web_api.web.Models;

namespace web_api.web.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoContext _context;
        private readonly ILogger<ToDoRepository> _logger;

        public ToDoRepository(ToDoContext context, ILogger<ToDoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IReadOnlyList<ToDo>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all ToDo items from database.");
            var toDos = await _context.ToDos.ToListAsync();
            return toDos.AsReadOnly();
        }

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            _logger.LogDebug("Fetching ToDo item with Id {Id} from database.", id);
            return await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ToDo> CreateAsync(ToDo toDo)
        {
            _logger.LogDebug("Adding new ToDo item to database.");
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
            _logger.LogDebug("Successfully added ToDo item with Id {Id} to database.", toDo.Id);
            return toDo;
        }

        public async Task<bool> UpdateAsync(ToDo toDo)
        {
            _logger.LogDebug("Updating ToDo item with Id {Id} in database.", toDo.Id);

            var existingToDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == toDo.Id);
            if (existingToDo == null)
            {
                _logger.LogDebug("ToDo item with Id {Id} not found in database.", toDo.Id);
                return false;
            }

            // Update properties
            existingToDo.Title = toDo.Title;
            existingToDo.IsCompleted = toDo.IsCompleted;
            existingToDo.Description = toDo.Description;

            await _context.SaveChangesAsync();
            _logger.LogDebug("Successfully updated ToDo item with Id {Id} in database.", toDo.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogDebug("Deleting ToDo item with Id {Id} from database.", id);

            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
            if (toDo == null)
            {
                _logger.LogDebug("ToDo item with Id {Id} not found in database for deletion.", id);
                return false;
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            _logger.LogDebug("Successfully deleted ToDo item with Id {Id} from database.", id);
            return true;
        }
    }
}
