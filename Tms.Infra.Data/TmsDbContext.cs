﻿using Microsoft.EntityFrameworkCore;
using Tms.Domain;

namespace Tms.Infra.Data
{
    public class TmsDbContext : DbContext
    {
        public TmsDbContext(DbContextOptions<TmsDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<Domain.Task> Tasks { get; set; }
        public DbSet<Domain.SubTask> SubTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Task>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Domain.Task>()
                .HasMany(x => x.SubTasks)
                .WithOne(x => x.ParentTask)
                .HasForeignKey(x => x.ParentTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Domain.SubTask>()
                .HasKey(x => new { x.ParentTaskId, x.TaskId });

            modelBuilder.Entity<Domain.Task>()
                .HasQueryFilter(x => x.DeleteDate == null);

            base.OnModelCreating(modelBuilder);
        }
    }
}