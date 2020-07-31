using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.Enums;
using Tms.Infra.CrossCutting.Helpers;

namespace Tms.Domain
{
    [Table("Task")]
    public class Task : BaseEntity
    {
        protected Task() : base()
        {
            this.SubTasks = new HashSet<SubTask>();
        }

        public Task(string name, string description) : this()
        {
            this.Name = name;
            this.Description = description;

            this.AlterTaskState(TaskStateEnum.Planned);
        }

        private string _name;
        private string _description;
        private DateTime? _startDate;
        private DateTime? _finishDate;
        private ICollection<SubTask> _subTasks;

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomainRulesException("A name is needed when creating a new Task");

                if (value.Length > 40)
                    throw new DomainRulesException("The Task's name can not be longer than 40 characteres");

                if (value.Length < 3)
                    throw new DomainRulesException("The Task's name can not be shorter than 3 characteres");

                _name = value;
            }
        }

        public string Description
        {
            get { return _description; }
            private set
            {
                if (value?.Length > 200)
                    throw new DomainRulesException("The Task's name can not be longer than 200 characteres");

                _description = value ?? throw new DomainRulesException("A description is needed when creating a new Task");
            }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            private set
            {
                if (value.HasValue & this.StartDate > DateTime.Now)
                    throw new DomainRulesException("Start date can not be setted in the future");

                _startDate = value; 
            }
        }

        public DateTime? FinishDate
        {
            get { return _finishDate; }
            private set 
            { 
                if(!this.StartDate.HasValue)
                    throw new DomainRulesException("You must informe a start date before setting finish date");
                else if (value.HasValue & this.StartDate > value.Value)
                    throw new DomainRulesException("Finish date can not be smaller than start date");

                _finishDate = value;
            }
        }

        public TaskStateEnum TaskState { get; private set; }

        public virtual ICollection<SubTask> SubTasks
        {
            get { return _subTasks; }
            private set { _subTasks = value; }
        }

        public void AddNewSubTask(SubTask subTask)
        {
            if (subTask == null)
                throw new BusinessLogicException("Please, informe a SubTask to be added");

            if (!this.SubTasks.Any(st => st == subTask))
                this.SubTasks.Add(subTask);

            this.SubTasks.Add(subTask);

            if (this.TaskState == TaskStateEnum.Completed)
                this.AlterTaskState(TaskStateEnum.Planned);
        }

        public void AlterTaskState(TaskStateEnum destinyState)
        {
            if(this.TaskState != destinyState)
            {
                switch (destinyState)
                {
                    case TaskStateEnum.Planned:
                        this.StartDate =
                            this.FinishDate = null;
                        break;
                    case TaskStateEnum.InProgress:
                        this.StartDate = DateTime.Now;
                        break;
                    case TaskStateEnum.Completed:
                        this.FinishDate = null;
                        break;
                    default:
                        throw new BusinessLogicException(
                            $"There's no defined process to set a Task's state to {EnumHelper.GetDescription<TaskStateEnum>(destinyState)}");
                }
            }
        }
    }
}