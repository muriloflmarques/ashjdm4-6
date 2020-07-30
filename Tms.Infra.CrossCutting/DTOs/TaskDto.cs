using System;
using System.Collections.Generic;
using System.Text;

namespace Tms.Infra.CrossCutting.DTOs
{
public   class TaskDto
    {
        public TaskDto ParentTask { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TaskState { get; set; }
        public string TaskStateText { get; set; }
        public TaskDto[] SubTasks { get; set; }
    }
}
