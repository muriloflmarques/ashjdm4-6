using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Task SelectById(int id) =>
            base.SelectById(this.AddDefaultIncludeIntoDbSet(this.GetDbSetAsNoTracking()), id);

        public IEnumerable<Task> SelectTop(IQueryable<Domain.Task> dbSet,
            Expression<Func<Domain.Task, bool>> query)
        {
            var tasks = base.SelectByQuery(dbSet, query)
                .Take(_defaultQueryConfigurations.LimitForSelectTop);

            if (_defaultQueryConfigurations.SelectTopDescending)
                return tasks.OrderByDescending(x => x.Id);

            return tasks.OrderBy(x => x.Id);
        }

        public override IQueryable<Domain.Task> GetDbSet() =>
            _tmsDbContext.Set<Domain.Task>();

        public override IQueryable<Domain.Task> GetDbSetAsNoTracking() =>
            _tmsDbContext.Set<Domain.Task>().AsNoTracking();

        public override IQueryable<Domain.Task> AddDefaultIncludeIntoDbSet(IQueryable<Domain.Task> dbSet) =>
             dbSet.Include(t => t.SubTasks).ThenInclude(sb => sb.ChildTask);
    }
}