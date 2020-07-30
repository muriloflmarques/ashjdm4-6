using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Tms.Infra.CrossCutting.Enums;

namespace Tms.Domain
{
    [Table("Task")]
    public partial class Task : BaseEntity
    {
        protected Task() { }

        public Task(string name, string description, DateTime? startDate,
            DateTime? finishDate) : base()
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.FinishDate = finishDate;

            this.SubTasks = new HashSet<SubTask>();
        }

        public Task(string name, string description)
         : this(name, description, null, null) { }

        private string _name;
        private string _description;
        private DateTime? _startDate;
        private DateTime? _finishDate;
        private TaskStateEnum _taskState;
        private ICollection<SubTask> _subTasks;

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

        public virtual ICollection<SubTask> SubTasks
        {
            get { return _subTasks; }
            private set { _subTasks = value; }
        }

        public void AddNewSubTask(SubTask subTask)
        {
            if (subTask == null)
                throw new Exception("Please, informe a SubTask to be added");

            if(!this.SubTasks.Any(st => st == subTask))
                this.SubTasks.Add(subTask);

            this.SubTasks.Add(subTask);
        }
    }
}