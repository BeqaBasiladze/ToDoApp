using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Low";
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime DueTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
