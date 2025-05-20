using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Data;
using ToDoApp.Data.Repository;
using ToDoApp.Domain.Entities;
using ToDoApp.Services;
using ToDoApp.Services.Interfaces;
using ToDoApp.Services.Services;
using ToDoApp.Services.Validator;



var service = new ServiceCollection();

service.AddDbContext<AppDbContext>(options => options
.UseSqlServer(@"Server=DESKTOP-K1OU8MM\MSSQLSERVER02;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True;"));

service.AddScoped(typeof(Repository<>));
service.AddScoped<ITaskService, TaskService>();
service.AddScoped<IValidator<TaskItem>, TaskItemValidator>();


var provider = service.BuildServiceProvider();


var taskService = provider.GetRequiredService<ITaskService>();


bool isExit = false;
while (isExit)
{

    Console.WriteLine("Выберите действие:");
    Console.WriteLine("1 - Показать все задачи");
    Console.WriteLine("2 - Поиск по заголовку");
    Console.WriteLine("3 - Добавить Задачу");
    Console.WriteLine("4 - Выйти");
    Console.Write("Введите номер действия: ");
    var operation = Console.ReadLine();
    Thread.Sleep(2000);
    Console.Clear();

    switch (operation)
    {
        case "1":
            var allTasks = await taskService.GetAllAsync();
            foreach (var task in allTasks)
            {
                Console.WriteLine($"{task.Id}. {task.Title} - Completed : {task.IsCompleted} Priority : {task.Priority} DueTime : {task.DueTime}");
            }
            break;
            case "2":
            Console.Write("Введите заголовок задачи для поиска: ");
            var title = Console.ReadLine();
            var tasksByTitle = await taskService.GetAllAsync();
            var foundTasks = tasksByTitle.Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            if (foundTasks.Count > 0)
            {
                foreach (var task in foundTasks)
                {
                    Console.WriteLine($"{task.Id}. {task.Title} - Completed : {task.IsCompleted} Priority : {task.Priority} DueTime : {task.DueTime}");
                }
            }
            else
            {
                Console.WriteLine("Задачи не найдены.");
            }
            break;
            case "3":
            Console.Write("Введите название задачи");
            var getTitle = Console.ReadLine();
            Console.Write("Введите описание задачи");
            var getDescription = Console.ReadLine();
            Console.Write("Введите priority задачи");
            var getPriority = Console.ReadLine();
            Console.Write("Введите DueDate задачи");
            var getDueDate = Convert.ToDateTime(Console.ReadLine());

            var newTask = new TaskItem
            {
                Title = getTitle,
                Description = getDescription,
                Priority = getPriority,
                DueTime = getDueDate,
                IsCompleted = false
            };

            await taskService.AddAsync(newTask);
            Console.WriteLine("Новая задача добавлена!");
            Thread.Sleep(2000);
            Console.Clear();
            break;
        case "4":
            isExit = true;
            break;
    }
}