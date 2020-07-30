using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Domain
{
    [Table("Task")]
    public class Task : BaseEntity
    {
        public Task(string name, string description, DateTime? startDate,
            DateTime? finishDate) : base()
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
        }

        public Task(string name, string description)
         : this(name, description, null, null) { }

        private Task _parentTask;
        private string _name;
        private string _description;
        private DateTime? _startDate;
        private DateTime? _finishDate;
        private TaskStateEnum _taskState;
        private List<Task> _subTasks;

        public Task ParentTask
        {
            get { return _parentTask; }
            private set { _parentTask = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            private set { _description = value; }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            private set { _startDate = value; }
        }

        public DateTime? FinishDate
        {
            get { return _finishDate; }
            private set { _finishDate = value; }
        }

        public TaskStateEnum TaskState
        {
            get { return _taskState; }
            private set { _taskState = value; }
        }

        public List<Task> SubTasks
        {
            get { return _subTasks; }
            private set { _subTasks = value; }
        }
    }
}