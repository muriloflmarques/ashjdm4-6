using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tms.Infra.Data
{
    public class TmsContext : DbContext
    {
        public TmsContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions) { }

        //private readonly string _connectionString;

        //public TmsContext(string connectionString)
        //{
        //    this._connectionString = connectionString;
        //}

        public DbSet<Domain.Task> Tasks { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(this._connectionString);

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
