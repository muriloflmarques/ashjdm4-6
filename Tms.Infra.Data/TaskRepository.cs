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

        /// <summary>
        /// Facade for SelectById using the default Include and AsNoTracking
        /// </summary>
        /// <returns>Task</returns>
        public Domain.Task SelectById(int id) =>
            base.SelectById(this.AddDefaultIncludeIntoDbSet(this.GetDbSetAsNoTracking()), id);

        /// <summary>
        /// Facade for accessing Select TOP with the system's default limite of results to any given query
        /// </summary>
        /// <returns>Task List</returns>
        public IEnumerable<Task> SelectTop(IQueryable<Domain.Task> dbSet,
            Expression<Func<Domain.Task, bool>> query)
        {
            //Using the system's default limite of results
            var tasks = base.SelectByQuery(dbSet, query)
                .Take(_defaultQueryConfigurations.LimitForSelectTop);

            if (_defaultQueryConfigurations.SelectTopDescending)
                return tasks.OrderByDescending(x => x.Id);

            return tasks.OrderBy(x => x.Id);
        }

        /// <summary>
        /// Override implementation to assure that this repository only proceeds to execute
        /// queries over the Task entity
        /// </summary>
        /// <returns> DbSet WITHOUT AsNoTracking </returns>
        public override IQueryable<Domain.Task> GetDbSet() =>
            _tmsDbContext.Set<Domain.Task>();

        /// <summary>
        /// Override implementation to assure that this repository only proceeds to execute
        /// queries over the Task entity
        /// </summary>
        /// <returns> DbSet WITH AsNoTracking </returns>
        public override IQueryable<Domain.Task> GetDbSetAsNoTracking() =>
            _tmsDbContext.Set<Domain.Task>().AsNoTracking();

        /// <summary>
        /// Override implementation to assure that this repository only proceeds to execute
        /// queries over the Task entity
        /// </summary>
        /// <returns> DbSet with most used Joins </returns>
        public override IQueryable<Domain.Task> AddDefaultIncludeIntoDbSet(IQueryable<Domain.Task> dbSet) =>
             dbSet.Include(t => t.SubTasks).ThenInclude(sb => sb.ChildTask);
    }
}