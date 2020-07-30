using System;

namespace Tms.Domain
{
    public class Task : BaseEntity
    {
        public Task(string name, string description, DateTime? startDate, DateTime? finishDate)
            : base()
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
        }

        public Task(string name, string description)
         : this(name, description, null, null) { }

        private string _name;
        private string _description;
        private DateTime? _startDate;
        private DateTime? _finishDate;
        //State

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

        //State
    }
}