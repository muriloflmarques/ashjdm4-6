using Tms.Domain;
using Tms.Infra.Data.Interface;
using Tms.Service.Interfaces;

namespace Tms.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            this._taskRepository = taskRepository;
        }

        public void Test()
        {
            Domain.Task task = new Domain.Task("Murilo", "Description");

            _taskRepository.Insert(task);
        }

        public void Test2()
        {
            Domain.Task task = new Domain.Task("Gabriela", "Description");
            Domain.Task taskNew = new Domain.Task("Gabriela Sub 1", "Description");

            SubTask subTask = new SubTask(task, taskNew);



            _taskRepository.Insert(task);
        }
    }
}