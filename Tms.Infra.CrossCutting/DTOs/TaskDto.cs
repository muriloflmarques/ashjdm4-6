using System;

namespace Tms.Infra.CrossCutting.DTOs
{
    //All DTOs are structs, it ensures that after populated the data will be always immutable
    public struct TaskDto
    {
        public int? ParentTaskId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TaskState { get; set; }
        public string TaskStateText { get; set; }
        public TaskDto[] SubTasks { get; set; }


        public DateTime CreationDate { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}