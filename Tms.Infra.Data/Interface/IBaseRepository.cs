using System;
using System.Collections.Generic;
using Tms.Domain;

namespace Tms.Infra.Data.Interface
{
    public interface IBaseRepository<T> : IDisposable where T : BaseEntity
    {
        void Insert(T obj);
        void Insert(IEnumerable<T> objs);
        void Update(T obj);
        void Update(IEnumerable<T> objs);
        void Delete(T obj);
        void Delete(IEnumerable<T> objs);

        T SelectById(int id);
        IEnumerable<T> SelectByQuery();
        T SelectFirstByQuery();
    }
}