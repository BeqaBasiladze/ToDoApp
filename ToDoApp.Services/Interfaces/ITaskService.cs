using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem item);
        Task UpdateAsync(TaskItem item);
        Task DeleteAsync(int id);
        Task<bool> MarkAsCompletedAsync(int id);

        Task<IEnumerable<TaskItem>> GetCompletedAsync();
        Task<IEnumerable<TaskItem>> GetPendingAsync();
        Task<IEnumerable<TaskItem>> GetByPriorityAndDueDateAsync(string priority, DateTime dueTime);
        Task<IEnumerable<TaskItem>> GetTasksCreatedBetweenAsync(DateTime from, DateTime to);
        Task<IEnumerable<TaskItem>> SearchByTitleAsync(string keyword);
        Task<(int total, int completed)> GetStatisticsAsync();
        Task<IEnumerable<TaskItem>> GetSortedByPriorityAsync();
    }
}
