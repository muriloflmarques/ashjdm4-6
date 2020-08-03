using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tms.Domain;
using Tms.Infra.CrossCutting.CustomException;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly TmsDbContext _tmsDbContext;

        public BaseRepository(TmsDbContext tmsDbContext) =>
            this._tmsDbContext = tmsDbContext;

        #region Basic CRUD for all classes from Domain inheriting from BaseEntity

        public void Update(T obj)
        {
            this.CheckIfNull(obj);

            obj.SetChangeDate();
            this._tmsDbContext.Update(obj);
        }

        public void Update(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                this.Update(obj);
        }

        public void Delete(T obj)
        {
            this.CheckIfNull(obj);

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
            this.CheckIfNull(obj);

            _tmsDbContext.Add(obj);
        }

        public void Insert(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                this.Insert(obj);
        }

        //Basic implementation of Select * from {BaseEntity} where Id = @id
        public T SelectById(IQueryable<T> dbSet, int id) =>
            dbSet.FirstOrDefault(x => x.Id == id);

        //Basic implementation of Select for any Domain inheriting from BaseEntity
        public IEnumerable<T> SelectByQuery(IQueryable<T> dbSet,
            Expression<Func<T, bool>> query) =>
            dbSet.Where(query).ToList();

        //Using FirstOrDefault over the basic implementation of Select for any Domain inheriting from BaseEntity
        public T SelectFirstByQuery(IQueryable<T> dbSet,
             Expression<Func<T, bool>> query) =>
            this.SelectByQuery(dbSet, query).FirstOrDefault();

        #endregion

        public void Dispose()
        {
            this._tmsDbContext?.Dispose();
        }

        private void CheckIfNull(T obj)
        {
            if (obj == null)
                throw new BusinessLogicException("Error while performing operation into Database");
        }

        //The following methods MUST be abstract to force the class who inherits BaseRepository to
        //have it's own DbSet to execute queries

        public abstract IQueryable<T> GetDbSet();

        public abstract IQueryable<T> GetDbSetAsNoTracking();

        public abstract IQueryable<T> AddDefaultIncludeIntoDbSet(IQueryable<T> dbSet);
    }
}