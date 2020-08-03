using System;
using System.ComponentModel.DataAnnotations;

namespace Tms.Domain
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.CreationDate = DateTime.Now;
            this.ChangeDate = null;
            this.DeleteDate = null;
        }

        public void SetChangeDate() =>
            this.ChangeDate = DateTime.Now;

        public void SetDeleteDate() =>
            this.DeleteDate = DateTime.Now;

        [Key]
        public int Id { get; private set; }

        //Dates to keep track of Creation, Last Update and Deletion of Domain entities

        //In the future it could also track when an entity suffered an update, who was the user who did it
        //and which data was updated from X to Y
        //This kind of log could be applied to specific entities and send to a queue to be estored in a
        //satalite system

        public DateTime CreationDate { get; private set; }
        public DateTime? ChangeDate { get; private set; }
        public DateTime? DeleteDate { get; private set; }
    }
}