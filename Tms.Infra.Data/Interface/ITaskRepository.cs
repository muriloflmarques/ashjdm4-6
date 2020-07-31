using System.Collections;
using System.Collections.Generic;

namespace Tms.Infra.Data.Interface
{
    public interface ITaskRepository : IBaseRepository<Domain.Task> 
    {
        Domain.Task FindParentTask(int subTaskId);
        IEnumerable<Domain.Task> SelectTop();
    }
}