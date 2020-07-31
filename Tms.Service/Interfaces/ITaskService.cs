using System.Collections.Generic;
using Tms.Infra.CrossCutting.DTOs;

namespace Tms.Service.Interfaces
{
    public interface ITaskService
    {
        void UpdateNewTask(int id, CreatingTaskDto creatingTaskDto);
        void CreateNewTask(CreatingTaskDto creatingTaskDto);
        void CreateNewSubTask(CreatingTaskDto creatingTaskDto);
        void DeleteTask(int id);
        IEnumerable<Domain.Task> SelectTasksWithoutSubtasks();
        IEnumerable<Domain.Task> SelectTasksWithSubtasks();

        TaskDto ConvertDomainToDto(Domain.Task task);
    }
}