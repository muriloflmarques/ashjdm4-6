using System.Collections.Generic;
using Tms.Infra.CrossCutting.DTOs;

namespace Tms.Service.Interfaces
{
    public interface ITaskService
    {
        void CreateNewTask(CreatingTaskDto creatingTaskDto);
        void CreateNewSubTask(CreatingTaskDto creatingTaskDto);
        void DeleteTask(int id);
        IEnumerable<Domain.Task> SelectTop();

        TaskDto ConvertDomainToDto(Domain.Task task);

    }
}