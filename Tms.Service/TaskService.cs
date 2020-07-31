using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Domain;
using Tms.Domain.PrivateMapper;
using Tms.Infra.CrossCutting.CustomException;
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

        public void CreateNewSubTask(CreatingTaskDto creatingTaskDto)
        {
            var parentTask = _taskRepository.SelectById(creatingTaskDto.ParentTaskId) ??
                throw new BusinessLogicException("To create a new SubTask a existing Task is needed");

            var childTask = creatingTaskDto.MapToDomain();

            var subTask = new SubTask(parentTask, childTask);

            parentTask.AddNewSubTask(subTask);

            _taskRepository.Insert(parentTask);
        }

        public void CreateNewTask(CreatingTaskDto creatingTaskDto)
        {
            var task = creatingTaskDto.MapToDomain();

            _taskRepository.Insert(task);
        }

        public void DeleteTask(int id)
        {
            var task = _taskRepository.SelectById(id) ??
                throw new BusinessLogicException("Error while deleting Task, Id informed didn't return any result");

            _taskRepository.Delete(task.SubTasks?.Select(sb => sb.ChildTask));
            _taskRepository.Delete(task);
        }

        public IEnumerable<Task> SelectTop()
        {
            return _taskRepository.SelectTop();
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