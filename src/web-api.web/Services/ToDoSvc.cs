using web_api.web.Models;
using web_api.web.Repositories;

namespace web_api.web.Services
{
    public class ToDoSvc
    {
        private readonly IToDoRepository _repository;
        private readonly ILogger<ToDoSvc> _logger;

        public ToDoSvc(IToDoRepository repository, ILogger<ToDoSvc> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<IReadOnlyList<ToDo>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all ToDo items.");
            return await _repository.GetAllAsync();
        }

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving ToDo item with Id {Id}.", id);
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ToDo> CreateAsync(ToDo toDo)
        {
            var createdToDo = await _repository.CreateAsync(toDo);
            _logger.LogInformation("Created new ToDo item with Id {Id}.", createdToDo.Id);
            return createdToDo;
        }

        public async Task<bool> UpdateAsync(ToDo toDo)
        {
            var success = await _repository.UpdateAsync(toDo);
            if (success)
            {
                _logger.LogInformation("Updated ToDo item with Id {Id}.", toDo.Id);
            }
            else
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for update.", toDo.Id);
            }
            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (success)
            {
                _logger.LogInformation("Deleted ToDo item with Id {Id}.", id);
            }
            else
            {
                _logger.LogWarning("ToDo item with Id {Id} not found for deletion.", id);
            }
            return success;
        }
    }
}
