using System.Linq;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.CrossCutting.Enums;
using Tms.Infra.CrossCutting.Helpers;

namespace Tms.Domain.PrivateMapper
{
    public static class PrivateMpper
    {
        public static TaskDto MapToDto(this Task task)
        {
            return 
                task == null ?
                new TaskDto()
                :
                new TaskDto()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    StartDate = task.StartDate,
                    FinishDate = task.FinishDate,
                    TaskState = (int)task.TaskState,
                    TaskStateText = EnumHelper.GetDescription<TaskStateEnum>(task.TaskState),

                    SubTasks = task.SubTasks.Select(st => st.ChildTask.MapToDto())?.ToArray() ?? new TaskDto[0],

                    CreationDate = task.CreationDate,
                    ChangeDate = task.ChangeDate
                };
        }

        public static Task MapToDomain(this CreatingTaskDto creatingTaskDto)
        {
            return new Task(creatingTaskDto.Name, creatingTaskDto.Description);
        }
    }
}