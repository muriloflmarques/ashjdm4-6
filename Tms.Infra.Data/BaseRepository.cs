using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tms.Domain;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly TmsDbContext _tmsDbContext;

        public BaseRepository(TmsDbContext tmsDbContext)
        {
            this._tmsDbContext = tmsDbContext;
        }

        public void Delete(T obj)
        {
            obj.SetDeleteDate();
            this.Update(obj);
        }

        public void Delete(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                this.Delete(obj);
        }

        public void Insert(T obj)
        {
            _tmsDbContext.Add(obj);
        }

        public void Insert(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                this.Insert(obj);
        }

        public virtual T SelectById(int id)
        {            
            return this.GetDbSet().FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> SelectByQuery(Expression<Func<T, bool>> query)
        {
            return this.GetDbSet().Where(query).ToList();
        }

        public virtual T SelectFirstByQuery(Expression<Func<T, bool>> query)
        {
            return this.SelectByQuery(query).FirstOrDefault();
        }

        public void Update(T obj)
        {
            obj.SetChangeDate();
            this._tmsDbContext.Update(obj);
        }

        public void Update(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                this.Update(obj);
        }

        public void Dispose()
        {
            this._tmsDbContext?.Dispose();
        }

        private void CheckIfNull(T obj)
        {
            if (obj == null) { }
        }

        private IQueryable<T> GetDbSet() => this._tmsDbContext.Set<T>().AsNoTracking();
    }
}