using Tms.Infra.CrossCutting.DTOs;

namespace Tms.Service.Interfaces
{
    public interface ITaskService
    {
        void CreateNewTask(TaskDto taskDto);
        void CreateNewSubTask(int parentTaskId, TaskDto taskDto);

        TaskDto ConvertDomainToDto(Domain.Task task);
        Domain.Task ConvertDtoToDomain(CreatingTaskDto dto);
    }
}