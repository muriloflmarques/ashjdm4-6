using System;
using Tms.Domain;
using Tms.Domain.PrivateMapper;
using Tms.Infra.CrossCutting.DTOs;
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

        public TaskDto ConvertDomainToDto(Task task) => 
            task.MapToDto();

        public Domain.Task ConvertDtoToDomain(CreatingTaskDto dto) =>
            dto.MapToDto();

        public void CreateNewSubTask(int parentTaskId, TaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public void CreateNewTask(TaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public void Test2()
        {
            Domain.Task task = new Domain.Task("Gabriela", "Description   " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            Domain.Task taskNew = new Domain.Task("Gabriela Sub 1", "Description   " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            SubTask subTask = new SubTask(task, taskNew);

            task.AddNewSubTask(subTask);

            _taskRepository.Insert(task);
        }
    }
}