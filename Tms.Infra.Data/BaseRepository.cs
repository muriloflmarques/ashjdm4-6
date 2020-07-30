using System;
using System.Collections.Generic;
using System.Text;
using Tms.Domain;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public void Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<T> objs)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<T> objs)
        {
            throw new NotImplementedException();
        }

        public T SelectById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SelectByQuery()
        {
            throw new NotImplementedException();
        }

        public T SelectFirstByQuery()
        {
            throw new NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<T> objs)
        {
            throw new NotImplementedException();
        }

        private void CheckIfNull(T obj)
        {
            if(obj == null) { }
        }
    }
}