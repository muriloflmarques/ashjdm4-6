using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tms.Domain;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class TaskRepository : BaseRepository<Domain.Task>, ITaskRepository
    {
        public TaskRepository(TmsDbContext tmsDbContext)
            : base(tmsDbContext) { }

        public Task FindParentTask(int subTaskId)
        {
            return GetDefaultDbSet()
                .FirstOrDefault(x => x.SubTasks.Any(st => st.Task.Id == subTaskId));
        }

        public override Domain.Task SelectById(int id)
        {
            return GetDefaultDbSet().FirstOrDefault(x => x.Id == id);
        }

        private IQueryable<Domain.Task> GetDefaultDbSet() =>
            _tmsDbContext.Set<Domain.Task>().AsNoTracking()
                .Include(x => x.SubTasks)
                    .ThenInclude(x => x.Task);
    }
}