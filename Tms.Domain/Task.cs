using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.CrossCutting.DTOs;
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

            this.ChangeTaskState(TaskStateEnum.Planned);
        }

        private string _name;
        private string _description;
        private DateTime? _startDate;
        private DateTime? _finishDate;
        private ICollection<SubTask> _subTasks;
        private TaskStateEnum _taskState;

        public int? ParentTaskId { get; private set; }

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
                    throw new DomainRulesException("The Task's name can not have less than 3 characteres");

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
                if (value.HasValue && this.StartDate > DateTime.Now)
                    throw new DomainRulesException("Start date can not be setted in the future");

                _startDate = value;
            }
        }

        public DateTime? FinishDate
        {
            get { return _finishDate; }
            private set
            {
                if (value != null)
                {
                    if (!this.StartDate.HasValue)
                        throw new DomainRulesException("You must informe a start date before setting a finish date");
                    else if (this.StartDate > value)
                        throw new DomainRulesException("Finish date can not be smaller than start date");
                }

                _finishDate = value;
            }
        }

        public TaskStateEnum TaskState
        {
            get { return _taskState; }
            private set
            {
                switch (value)
                {
                    //A Planned Task hasn't started nor finished
                    case TaskStateEnum.Planned:
                        this.StartDate =
                            this.FinishDate = null;
                        break;

                    //A Task in InProgress hasn't finished but has started
                    case TaskStateEnum.InProgress:
                        this.StartDate = DateTime.Now;
                        this.FinishDate = null;
                        break;

                    //A Completed Task has started and finished
                    case TaskStateEnum.Completed:
                        this.FinishDate = DateTime.Now;
                        break;

                    //In case that the destiny State is not valid
                    default:
                        this.ThrowExceptionDestinyStateNotFound(value);
                        break;
                }

                _taskState = value;
            }
        }

        public virtual ICollection<SubTask> SubTasks
        {
            get { return _subTasks; }
            private set { _subTasks = value; }
        }

        public void AddSubTask(SubTask subTask)
        {
            if (subTask == null)
                throw new BusinessLogicException("Please, informe a SubTask to be added");

            if (this.ParentTaskId.HasValue)
                throw new BusinessLogicException("A SubTask can not hold any SubTask");

            if (subTask.ParentTask != this)
                throw new BusinessLogicException("The current SubTask belongs to another Task");

            this.SubTasks.Add(subTask);

            subTask.ChildTask.AddParentTaskId(this);


            //When adding a New SubTask it's essencial that we assure the corret Parent's State

            switch (this.TaskState)
            {
                //If Parent's State still Planned
                case TaskStateEnum.Planned:
                    //then it can go to InProgress if the new SubStask has InProgress as it's State
                    if (subTask.ChildTask.TaskState == TaskStateEnum.InProgress)
                        this.ChangeTaskState(TaskStateEnum.InProgress);

                    //or it can go to Completed (following the status progression) if the new
                    //SubStask has Completed as it's State
                    else if (subTask.ChildTask.TaskState == TaskStateEnum.Completed)
                    {
                        this.ChangeTaskState(TaskStateEnum.InProgress);
                        this.ChangeTaskState(TaskStateEnum.Completed);
                    }

                    break;

                //If Parent's State is InProgressit means that theres a SubTask in InProgress Status or any SubTask at all
                case TaskStateEnum.InProgress:
                    //and it doesn't have any SubTask InProgress, than the Parent should follow the SubTask's Status
                    if (subTask.ChildTask.TaskState != TaskStateEnum.InProgress
                        && this.SubTasks.All(st => st.ChildTask.TaskState != TaskStateEnum.InProgress))
                        this.ChangeTaskState(subTask.ChildTask.TaskState);
                    break;

                //If Parent's State is Complete (it means all it SubTasks are also in Complete Status)
                case TaskStateEnum.Completed:
                    //and the added SubTask is not Complete than the Parent should have the same Status as the SubTask
                    if (subTask.ChildTask.TaskState != TaskStateEnum.Completed)
                        this.ChangeTaskState(subTask.ChildTask.TaskState);
                    break;
                default:
                    break;
            }
        }

        public SubTask RemoveSubTask(Task task)
        {
            if (task == null)
                throw new BusinessLogicException("Please, informe a SubTask to be removed");

            if (this.SubTasks.Count <= 0)
                throw new BusinessLogicException("The current Task has no SubTasks");

            var subTask = this.SubTasks.FirstOrDefault(st => st.ChildTask.Id == task.Id)
                ??
                throw new BusinessLogicException("The SubTask does not belong to the current Task");

            this.SubTasks.Remove(subTask);

            subTask.ChildTask.RemoveParentTaskId();

            //When changing one SubTask's State the Parent Task should be aware of it's own State
            this.TaskState = GetWhichStateTheParentTaskShouldBe();

            return subTask;
        }

        public void ChangeTaskState(TaskStateEnum destinyState)
        {
            //If a Task has Children then it's state is determined by it's childen's state
            if (this.SubTasks.Any())
                throw new DomainRulesException("A parent Task's state can only be changed by changing it's children's Tasks state");

            if (this.TaskState != destinyState)
                this.TaskState = destinyState;
        }

        public void ChangeSubTaskState(Task taskToChange, TaskStateEnum destinyState)
        {
            if (taskToChange == null)
                throw new BusinessLogicException("Please, informe a SubTask to be changed");

            var subTask = this.SubTasks.FirstOrDefault(st => st.ChildTask.Id == taskToChange.Id)
                ??
                throw new BusinessLogicException("The SubTask does not belong to the current Task");

            subTask.ChildTask.ChangeTaskState(destinyState);

            //When changing one SubTask's State the Parent Task should be aware of it's own State
            this.TaskState = GetWhichStateTheParentTaskShouldBe();
        }

        public void AddParentTaskId(Task task)
        {
            if (this.Id == task?.Id)
                throw new DomainRulesException("A Task can not be it's own Parent");

            this.ParentTaskId = task?.Id;
        }

        public void RemoveParentTaskId() =>
            this.ParentTaskId = null;

        public void UpdateNameAndDescription(CreatingTaskDto creatingTaskDto)
        {
            this.Name = creatingTaskDto.Name;
            this.Description = creatingTaskDto.Description;
        }

        private TaskStateEnum GetWhichStateTheParentTaskShouldBe()
        {
            //Checking "Any" before of "All" maximixe the changes of not having to go through the whole collection

            //The Parent Task is InProgress if it has ANY Children Task in InProgress
            if (this.SubTasks.Any(st => st.ChildTask.TaskState == TaskStateEnum.InProgress))
                return TaskStateEnum.InProgress;

            //The Parent Task is Completed if it has ALL Children Task Completed
            else if (this.SubTasks.All(st => st.ChildTask.TaskState == TaskStateEnum.Completed))
                return TaskStateEnum.Completed;

            //The Parent Task is Planned in all the other cases
            else
                return TaskStateEnum.Planned;
        }

        private void ThrowExceptionDestinyStateNotFound(TaskStateEnum destinyState) =>
            throw new BusinessLogicException($"The informed status ({(int)destinyState}) does not exist");
    }
}