using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Services.Validator
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("\"Название задачи обязательно\"")
                .MaximumLength(100).WithMessage("Название не может быть длиннее 100 символов");
        }
    }
}
