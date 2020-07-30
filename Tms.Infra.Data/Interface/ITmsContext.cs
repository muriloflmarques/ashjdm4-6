using Microsoft.EntityFrameworkCore;

namespace Tms.Infra.Data.Interface
{
    public interface ITmsContext
    {
        public DbSet<Domain.Task> Tasks { get; set; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}