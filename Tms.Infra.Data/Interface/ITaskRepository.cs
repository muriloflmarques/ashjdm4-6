using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tms.Infra.Data.Interface
{
    public interface ITaskRepository : IBaseRepository<Domain.Task> 
    {
        Domain.Task FindParentTask(int subTaskId);
        IEnumerable<Domain.Task> SelectTop(IQueryable<Domain.Task> dbSet, Expression<Func<Domain.Task, bool>> query);
        Domain.Task SelectById(int id);
    }
}