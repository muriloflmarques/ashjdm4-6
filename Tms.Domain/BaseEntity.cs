using System;
using System.Collections.Generic;
using System.Text;

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

        public void SetChangeDate() => this.ChangeDate = DateTime.Now;

        public void SetDeleteDate() => this.DeleteDate = DateTime.Now;

        public string Id { get; set; }

        public DateTime CreationDate { get; private set; }
        public DateTime? ChangeDate { get; private set; }
        public DateTime? DeleteDate { get; private set; }
    }
}