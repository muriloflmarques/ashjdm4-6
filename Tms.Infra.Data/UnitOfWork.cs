using Microsoft.EntityFrameworkCore.Storage;
using System;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TmsDbContext _tmsDbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(TmsDbContext tmsDbContext)
        {
            this._tmsDbContext = tmsDbContext;
        }

        public void BeginTransaction()
        {
            _transaction =
                _tmsDbContext.Database.CurrentTransaction ?? _tmsDbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                this.BeginTransaction();
                _tmsDbContext.SaveChanges();
                _transaction.Commit();
            }
            catch(Exception ex)
            {
                this.Rollback();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}