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
        private readonly TmsDbContext _tmsDbContext;
        private readonly IQueryable<T> _entity;

        public BaseRepository(TmsDbContext tmsDbContext)
        {
            this._tmsDbContext = tmsDbContext;
            this._entity = this._tmsDbContext.Set<T>().AsNoTracking();
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

        public T SelectById(int id)
        {
            return _entity.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> SelectByQuery(Expression<Func<T, bool>> query)
        {
            return _entity.Where(query).ToList();
        }

        public T SelectFirstByQuery(Expression<Func<T, bool>> query)
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
    }
}