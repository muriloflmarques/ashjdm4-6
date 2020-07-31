using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tms.Domain;
using Tms.Domain.PrivateMapper;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.Data;
using Tms.Infra.Data.Interface;
using Tms.Service.Interfaces;
//using Microsoft.EntityFrameworkCore.Relational;

namespace Tms.Service
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITaskRepository _taskRepository;

        public TaskService(IUnitOfWork uow, ITaskRepository taskRepository)
        {
            this._uow = uow;
            this._taskRepository = taskRepository;
        }

        public TaskDto ConvertDomainToDto(Task task) =>
            task.MapToDto();

        private void CreateSqlCommandToDeleteSubTask(int parentTaskId, int childTaskId)
        {
            const string paramNameParentTaskId = "@ParentTaskId";
            const string paramNameChildTaskId = "@ChildTaskId";

            var paramParentTaskId = new SqlParameter(paramNameParentTaskId, parentTaskId);
            var paramChildTaskId = new SqlParameter(paramNameChildTaskId, childTaskId);

            var rawSqlString = new RawSqlString($"DELETE SubTasks WHERE ParentTaskId = {paramNameParentTaskId} and ChildTaskId = {paramNameChildTaskId} ");

            var rawSQlCommand = new RawSQlCommand(rawSqlString,
                new object[] { paramParentTaskId, paramChildTaskId });

            _uow.AddRawCommand(rawSQlCommand);
        }

        public void UpdateNewTask(int id, CreatingTaskDto creatingTaskDto)
        {
            var updatedTask = _taskRepository.SelectById(
                 _taskRepository.GetDbSetWithDefaultInclude(), id)
                 ??
                 throw new BusinessLogicException("Error while updating Task, Id informed didn't return any result");

            updatedTask.UpdateNameAndDescription(creatingTaskDto);

            if (creatingTaskDto.ParentTaskId > 0 &&
                (updatedTask.ParentTaskId ?? 0) != creatingTaskDto.ParentTaskId)
            {
                if (updatedTask.ParentTaskId.HasValue)
                {
                    var currentParentTask = _taskRepository.SelectById(
                        _taskRepository.GetDbSetWithDefaultInclude(), updatedTask.ParentTaskId.Value)
                        ??
                        throw new BusinessLogicException("Error while updating Task, the curent parent of the updated Task could not be found");

                    var removedSubTask = currentParentTask.RemoveSubTask(updatedTask);

                    _taskRepository.Update(currentParentTask);

                    CreateSqlCommandToDeleteSubTask(parentTaskId: currentParentTask.Id,
                        childTaskId: updatedTask.Id);
                }

                var newParentTask = _taskRepository.SelectById(
                   _taskRepository.GetDbSetWithDefaultInclude(), creatingTaskDto.ParentTaskId)
                   ??
                   throw new BusinessLogicException("Error while updating Task, could not find the new parent Task");

                this.AddSubTaskToTask(newParentTask, updatedTask);
            }

            _taskRepository.Update(updatedTask);
        }

        public void CreateNewTask(CreatingTaskDto creatingTaskDto)
        {
            var task = creatingTaskDto.MapToDomain();

            _taskRepository.Insert(task);
        }

        public void CreateNewSubTask(CreatingTaskDto creatingTaskDto)
        {
            var parentTask = _taskRepository.SelectById(
                _taskRepository.GetDbSetWithDefaultInclude(),
                creatingTaskDto.ParentTaskId)
                ??
                throw new BusinessLogicException("To create a new SubTask a existing Task is needed");

            var childTask = creatingTaskDto.MapToDomain();

            this.AddSubTaskToTask(parentTask, childTask);
        }

        private void AddSubTaskToTask(Domain.Task parentTask, Domain.Task childTask)
        {
            var subTask = new SubTask(parentTask, childTask);

            parentTask.AddSubTask(subTask);

            _taskRepository.Update(parentTask);
        }

        public void DeleteTask(int id)
        {
            var task = _taskRepository.SelectById(
                _taskRepository.GetDbSetWithDefaultInclude(), id)
                ??
                throw new BusinessLogicException("Error while deleting Task, Id informed didn't return any result");

            _taskRepository.Delete(task.SubTasks?.Select(sb => sb.ChildTask));
            _taskRepository.Delete(task);
        }

        public IEnumerable<Domain.Task> SelectTasksWithoutSubtasks()
        {
            return _taskRepository.SelectTop(
                _taskRepository.GetDbSet(),
                task => task.Id > 0);
        }

        public IEnumerable<Domain.Task> SelectTasksWithSubtasks()
        {
            return _taskRepository.SelectTop(
                _taskRepository.GetDbSetWithDefaultInclude(),
                task => !task.ParentTaskId.HasValue);
        }
    }
}