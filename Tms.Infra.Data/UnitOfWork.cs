using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Tms.Infra.Data.Interface;

namespace Tms.Infra.Data
{
    public class RawSQlCommand
    {
        public RawSQlCommand(RawSqlString rawSqlString, object[] paramsForSql = null)
        {
            this.RawSqlString = rawSqlString;
            this.ParamsForSql = paramsForSql;
        }

        public RawSqlString RawSqlString { get; protected set; }
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

        public void cacete(int parentTaskId, int childTaskId)
        {
            _tmsDbContext.Database.ExecuteSqlRaw("");

            //const string paramNameParentTaskId = "@ParentTaskId";
            //const string paramNameChildTaskId = "@ParentTaskId";

            //_tmsDbContext.Database.CommitTransaction

            //var paramParentTaskId = new SqlParameter(paramNameParentTaskId, parentTaskId);
            //var paramChildTaskId = new SqlParameter(paramNameChildTaskId, childTaskId);

            //var commandText = $"DELETE SubTasks WHERE ParentTaskId = {paramNameParentTaskId} and ChildTaskId = {paramNameChildTaskId} ";
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
                    {
                        _tmsDbContext.Database
                                .ExecuteSqlCommand(rawCommand.RawSqlString, rawCommand.ParamsForSql);
                    }
                    else
                    {
                        _tmsDbContext.Database
                                .ExecuteSqlCommand(rawCommand.RawSqlString);
                    }
                }

                _tmsDbContext.SaveChanges();
                _transaction.Commit();
            }
            catch(Exception ex)
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