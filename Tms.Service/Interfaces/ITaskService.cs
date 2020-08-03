using System.Collections.Generic;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Service.Interfaces
{
    public interface ITaskService
    {
        TaskDto ChangeTaskState(int id, TaskStateEnum destinyState, bool commit = false);
        TaskDto UpdateTask(int id, CreatingTaskDto creatingTaskDto, bool commit = false);
        TaskDto CreateNewTask(CreatingTaskDto creatingTaskDto, bool commit = false);
        TaskDto CreateNewSubTask(CreatingTaskDto creatingTaskDto, bool commit = false);
        void DeleteTask(int id);
        IEnumerable<TaskDto> SelectTasksWithoutSubtasks();
        IEnumerable<TaskDto> SelectTasksWithSubtasks();
    }
}