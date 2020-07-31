using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Tms.Domain;
using Tms.Infra.CrossCutting.Configurations;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class TaskRepository : BaseRepository<Domain.Task>, ITaskRepository
    {
        private readonly DefaultQueryConfigurations _defaultQueryConfigurations;

        public TaskRepository(IOptions<DefaultQueryConfigurations> defaultQueryConfigurations,
            TmsDbContext tmsDbContext) : base(tmsDbContext)
        {
            this._defaultQueryConfigurations = defaultQueryConfigurations.Value;
        }

        public Task FindParentTask(int subTaskId)
        {
            return GetDefaultDbSet()
                .FirstOrDefault(x => x.SubTasks.Any(st => st.ChildTask.Id == subTaskId));
        }

        public override Domain.Task SelectById(int id)
        {
            return GetDefaultDbSet().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Task> SelectTop()
        {
            var query = GetDefaultDbSet().Take(_defaultQueryConfigurations.LimitForSelectTop);

            if (_defaultQueryConfigurations.SelectTopDescending)
                return query.OrderByDescending(x => x.Id);

            return query.OrderBy(x => x.Id);
        }

        private IQueryable<Domain.Task> GetDefaultDbSet() =>
            _tmsDbContext.Set<Domain.Task>().AsNoTracking()
                .Include(x => x.SubTasks)
                    .ThenInclude(x => x.ChildTask);
    }
}