using System.Linq;
using Tms.Infra.CrossCutting.DTOs;
using Tms.Infra.CrossCutting.Enums;
using Tms.Infra.CrossCutting.Helpers;

namespace Tms.Domain.PrivateMapper
{
    public static class TaskPrivateMpper
    {
        static TaskDto MapToDto(this Task task)
        {
            return new TaskDto()
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                FinishDate = task.FinishDate,
                TaskState = (int)task.TaskState,
                TaskStateText = EnumHelper.GetDescription<TaskStateEnum>(task.TaskState),

                SubTasks = task.SubTasks.Select(st => st.Task.MapToDto()).ToArray(),

                CreationDate = task.CreationDate,
                ChangeDate = task.ChangeDate
            };
        }
    }
}