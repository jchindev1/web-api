using web_api.web.Models;

namespace web_api.web.Services
{
    public class ToDoSvc
    {
        private readonly List<ToDo> _toDos = new();
        private readonly ILogger<ToDoSvc> _logger;

        public ToDoSvc(ILogger<ToDoSvc> logger)
        {
            _logger = logger;
        }

        public Task<IReadOnlyList<ToDo>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all ToDo items.");
            return Task.FromResult((IReadOnlyList<ToDo>)_toDos.AsReadOnly());
        }

        public Task<ToDo?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving ToDo item with Id {Id}.", id);
            var todo = _toDos.Find(t => t.Id == id);
            return Task.FromResult(todo);
        }

        public Task<ToDo> CreateAsync(ToDo toDo)
        {
            toDo.Id = _toDos.Count > 0 ? _toDos[^1].Id + 1 : 1;
            _toDos.Add(toDo);
            _logger.LogInformation("Created new ToDo item with Id {Id}.", toDo.Id);
            return Task.FromResult(toDo);
        }

        public Task<bool> UpdateAsync(ToDo toDo)
        {
            var index = _toDos.FindIndex(t => t.Id == toDo.Id);
            if (index == -1)
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for update.", toDo.Id);
                return Task.FromResult(false);
            }
            _toDos[index] = toDo;
            _logger.LogInformation("Updated ToDo item with Id {Id}.", toDo.Id);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var toDo = _toDos.Find(t => t.Id == id);
            if (toDo == null)
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for deletion.", id);
                return Task.FromResult(false);
            }
            _toDos.Remove(toDo);
            _logger.LogInformation("Deleted ToDo item with Id {Id}.", id);
            return Task.FromResult(true);
        }
    }
}
