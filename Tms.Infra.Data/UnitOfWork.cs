using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    /// <summary>
    /// As the Task entity is self-referenced through the SubTask entity it's
    /// not possible to have it delete on the common way without triggering a
    /// possible redundant delete using cascade, because of that I have 
    /// implemented this class to append delete command to the DbContext 
    /// transaction so it would only be performed when the commit occours
    /// </summary>
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

    /// <summary>
    /// Self implemented Unit Of Work to fully commit all changes
    /// </summary>
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
                    //execute SQL Command WITH params
                    if (rawCommand.ParamsForSql != null)                    
                        _tmsDbContext.Database.ExecuteSqlRaw(rawCommand.RawSqlString, rawCommand.ParamsForSql);
                    
                    //execute SQL Command WITHOUT params
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
                //Cleand the current commands so they won't be executed more than once
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