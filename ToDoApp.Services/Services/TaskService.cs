using FluentValidation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Repository;
using ToDoApp.Domain.Entities;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly Repository<TaskItem> _repository;
        private readonly IValidator<TaskItem> _validator;

        public TaskService(Repository<TaskItem> repository, IValidator<TaskItem> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task AddAsync(TaskItem item)
        {
            var result = await _validator.ValidateAsync(item);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            await _repository.AddAsync(item);
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task UpdateAsync(TaskItem item)
        {
            var result = await _validator.ValidateAsync(item);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            await _repository.UpdateAsync(item);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<TaskItem?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<TaskItem>> GetCompletedAsync()
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t => t.IsCompleted);
        }

        public async Task<IEnumerable<TaskItem>> GetPendingAsync()
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t => !t.IsCompleted);
        }

        public async Task<IEnumerable<TaskItem>> GetByPriorityAndDueDateAsync(string priority, DateTime dueTime)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t=>t.Priority == priority && t.DueTime == dueTime);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksCreatedBetweenAsync(DateTime from, DateTime to)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t=>t.CreateAt >=from && t.CreateAt <= to);
        }

        public async Task<IEnumerable<TaskItem>> SearchByTitleAsync(string keyword)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(t=>t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if(result != null)
            {
                result.IsCompleted = true;
                await _repository.UpdateAsync(result);
                return true;
            }
            return false;
        }

        public async Task<(int total, int completed)> GetStatisticsAsync()
        {
            var all = await _repository.GetAllAsync();
            var total = all.Count();
            var completed = all.Where(t=>t.IsCompleted).Count();

            return (total, completed);
        }

        public async Task<IEnumerable<TaskItem>> GetSortedByPriorityAsync()
        {
            var all = await _repository.GetAllAsync();

            var priorityOrder = new Dictionary<string, int>
            {
                ["Hight"] = 1,
                ["Medium"] = 2,
                ["Low"] = 3
            };

            return all.OrderBy(t => priorityOrder.ContainsKey(t.Priority) ? priorityOrder[t.Priority] : int.MaxValue);
        }
    }
}
