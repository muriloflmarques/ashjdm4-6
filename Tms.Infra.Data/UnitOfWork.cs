using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class RawSQlCommand
    {
        public RawSQlCommand(string rawSqlString, object[] paramsForSql = null)
        {
            this.RawSqlString = rawSqlString;
            this.ParamsForSql = paramsForSql;
        }

        public string RawSqlString { get; protected set; }
        public object[] ParamsForSql { get; protected set; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TmsDbContext _tmsDbContext;
        private IDbContextTransaction _transaction;

        public List<RawSQlCommand> RawSQlCommandList { get; private set; } = new List<RawSQlCommand>();

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

                foreach (var rawCommand in RawSQlCommandList)
                {
                    if (rawCommand.ParamsForSql != null)                    
                        _tmsDbContext.Database.ExecuteSqlRaw(rawCommand.RawSqlString, rawCommand.ParamsForSql);
                    
                    else                   
                        _tmsDbContext.Database.ExecuteSqlRaw(rawCommand.RawSqlString);
                    
                }

                _tmsDbContext.SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                this.Rollback();
            }
            finally
            {
                this.RawSQlCommandList = new List<RawSQlCommand>();
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public void AddRawCommand(RawSQlCommand rawSQlCommand) =>
            RawSQlCommandList.Add(rawSQlCommand);
    }
}