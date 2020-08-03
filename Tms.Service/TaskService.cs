using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tms.Domain;
using Tms.Domain.PrivateMapper;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.CrossCutting.Enums;
using Tms.Infra.Data;
using Tms.Infra.Data.Interface;
using Tms.Service.Interfaces;

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

        /// <summary>
        /// Facade to make conversion between Domain and DTO accessible
        /// </summary>
        /// <param name="task"></param>
        /// <returns>TaskDto fully filled with data</returns>
        public TaskDto ConvertDomainToDto(Task task) =>
            task.MapToDto();

        /// <summary>
        /// Most of the time the queries over Task entity will have the same Includes (for Join)
        /// and will be setted as AsNoTracking so this method prevents many equal lines to come
        /// </summary>
        /// <returns>IQueryable with proper dbSet AsNoTracking</returns>
        private IQueryable<Domain.Task> GetDbSetAsNoTrackingWithDefaultIncludes() =>
            _taskRepository.AddDefaultIncludeIntoDbSet(_taskRepository.GetDbSetAsNoTracking());

        /// <summary>
        /// This method registers in the current SQL Transaction a command to perform the
        /// Delete of a SubTask entity
        /// </summary>
        private void CreateSqlCommandToDeleteSubTask(int parentTaskId, int childTaskId)
        {
            const string paramNameParentTaskId = "@ParentTaskId";
            const string paramNameChildTaskId = "@ChildTaskId";

            var paramParentTaskId = new SqlParameter(paramNameParentTaskId, parentTaskId);
            var paramChildTaskId = new SqlParameter(paramNameChildTaskId, childTaskId);

            var rawSqlString = $"DELETE SubTasks WHERE ParentTaskId = {paramNameParentTaskId} and ChildTaskId = {paramNameChildTaskId} ";

            var rawSQlCommand = new RawSQlCommand(rawSqlString,
                new object[] { paramParentTaskId, paramChildTaskId });

            //Include comand to a Listof commands that will be checked when the Commit is performed
            _uow.AddRawCommand(rawSQlCommand);
        }

        /// <summary>
        /// Method for updating a Task
        /// </summary>
        public void UpdateTask(int id, CreatingTaskDto creatingTaskDto)
        {
            //Find the Task that will be updated or Throw Exception
            var updatedTask = _taskRepository.SelectById(
                 this.GetDbSetAsNoTrackingWithDefaultIncludes(), id)
                 ??
                 throw new BusinessLogicException("Error while updating Task, informed Id didn't return any result");

            updatedTask.UpdateNameAndDescription(creatingTaskDto);

            //If DTO informs that this Task is a Child Task and that it's Parent has changed
            if (creatingTaskDto.ParentTaskId > 0 &&
                (updatedTask.ParentTaskId ?? 0) != creatingTaskDto.ParentTaskId)
            {
                //if the Child Task already have a Parent
                if (updatedTask.ParentTaskId.HasValue)
                {
                    //Find the Parent Task of the updated Task or Throw Exception
                    var currentParentTask = _taskRepository.SelectById(
                         this.GetDbSetAsNoTrackingWithDefaultIncludes(), updatedTask.ParentTaskId.Value)
                        ??
                        throw new BusinessLogicException("Error while updating Task, the curent parent of the updated Task could not be found");

                    var removedSubTask = currentParentTask.RemoveSubTask(updatedTask);

                    _taskRepository.Update(currentParentTask);

                    //And then remove it's relation with the Child Task
                    CreateSqlCommandToDeleteSubTask(parentTaskId: currentParentTask.Id,
                        childTaskId: updatedTask.Id);
                }

                //Find the NEW Parent Task of the updated Task or Throw Exception
                var newParentTask = _taskRepository.SelectById(
                    this.GetDbSetAsNoTrackingWithDefaultIncludes(), creatingTaskDto.ParentTaskId)
                   ??
                   throw new BusinessLogicException("Error while updating Task, could not find the new parent Task");

                //Add the Child Task to the new Parent
                this.AddSubTaskToTask(newParentTask, updatedTask);
            }

            //Set all the changes to be commited by the Unit of Work
            _taskRepository.Update(updatedTask);
        }

        /// <summary>
        /// Method to update a Task's State
        /// </summary>
        public void ChangeTaskState(int id, TaskStateEnum destinyState)
        {
            //Find the Task that will be updated or Throw Exception
            var taskToUpdate = _taskRepository.SelectById(
                 this.GetDbSetAsNoTrackingWithDefaultIncludes(), id)
                 ??
                 throw new BusinessLogicException("Error while changing Task's state, informed Id didn't return any result");

            //If the Task has a Parent
            if (taskToUpdate.ParentTaskId.HasValue)
            {
                var dbSet = this.GetDbSetAsNoTrackingWithDefaultIncludes();

                //Then find it's Parent or Throw Exception
                var parentTask =
                    _taskRepository.SelectById(dbSet, taskToUpdate.ParentTaskId.Value)
                    ??
                    throw new BusinessLogicException("Error while changing Task's state, parent Task not found");

                //And change the Child Task's State through the Parent's methods
                parentTask.ChangeSubTaskState(taskToUpdate, destinyState);

                //Set all the changes to be commited by the Unit of Work
                _taskRepository.Update(parentTask);
            }
            else
            {
                //If the Task has no Parent then it's responsible for editing it's own State
                taskToUpdate.ChangeTaskState(destinyState);

                //Set all the changes to be commited by the Unit of Work
                _taskRepository.Update(taskToUpdate);
            }
        }

        /// <summary>
        /// Method to create a new Task
        /// </summary>
        public void CreateNewTask(CreatingTaskDto creatingTaskDto)
        {
            //Use the private map to Map the DTO from Application to a Domain entity
            var task = creatingTaskDto.MapToDomain();

            //Set all the changes to be commited by the Unit of Work
            _taskRepository.Insert(task);
        }

        /// <summary>
        /// Method to create a new Task and immediatily associate it as a Child to another Task
        /// </summary>
        public void CreateNewSubTask(CreatingTaskDto creatingTaskDto)
        {
            //Select the informed Parent Task or Throw Exception
            var parentTask = _taskRepository.SelectById(
                this.GetDbSetAsNoTrackingWithDefaultIncludes(),
                creatingTaskDto.ParentTaskId)
                ??
                throw new BusinessLogicException("To create a new SubTask a existing Task is needed");

            //Use the private map to Map the DTO from Application to a Domain entity
            var childTask = creatingTaskDto.MapToDomain();

            //Associates the new SubTask to the informed Parent Task
            this.AddSubTaskToTask(parentTask, childTask);

            //Set all the changes to be commited by the Unit of Work
            _taskRepository.Update(parentTask);
        }

        /// <summary>
        /// Facade to Add a SubTask to a Task
        /// </summary>
        private void AddSubTaskToTask(Domain.Task parentTask, Domain.Task childTask)
        {
            var subTask = new SubTask(parentTask, childTask);

            parentTask.AddSubTask(subTask);

            _taskRepository.Update(parentTask);
        }

        /// <summary>
        /// Method to delete a Task and all it SubTasks
        /// </summary>
        public void DeleteTask(int id)
        {
            //Find the Task to Delete or Throw Exception
            var task = _taskRepository.SelectById(
                this.GetDbSetAsNoTrackingWithDefaultIncludes(), id)
                ??
                throw new BusinessLogicException("Error while deleting Task, informed Id didn't return any result");

            //Set the Task to be deleted on next commit
            _taskRepository.Delete(task);

            //For all SubTasks of the deleted Task
            task.SubTasks?.Select(st =>
            {
                //Set the ChildTask to be deleted on next commit
                _taskRepository.Delete(st.ChildTask);

                //And then remove it's relation with the Parent Task
                this.CreateSqlCommandToDeleteSubTask(
                    parentTaskId: st.ParentTask.Id,
                    childTaskId: st.ChildTask.Id);

                return st;
            });
        }

        /// <summary>
        /// Method to Select all the Tasks without hierarchy, it said, there won't be any Join with
        /// SubTask the determine if the Task is a Parent or a Child
        /// </summary>
        public IEnumerable<Domain.Task> SelectTasksWithoutSubtasks()
        {
            return _taskRepository.SelectTop(
                _taskRepository.GetDbSetAsNoTracking(),
                task => task.Id > 0);
        }

        /// <summary>
        /// Method to Select all the Tasks WITH hierarchy, this way if a Task is a Child it will not
        /// appear on the root element, being only shown as a element of it's parent
        /// </summary>
        public IEnumerable<Domain.Task> SelectTasksWithSubtasks()
        {
            return _taskRepository.SelectTop(
                this.GetDbSetAsNoTrackingWithDefaultIncludes(),
                task => !task.ParentTaskId.HasValue);
        }
    }
}