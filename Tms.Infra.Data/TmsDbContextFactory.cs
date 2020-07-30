using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tms.Infra.Data
{
    public class TmsDbContextFactory : IDesignTimeDbContextFactory<TmsDbContext>
    {
        public TmsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TmsDbContext>();
            optionsBuilder.UseSqlServer("server=localhost;database=TmsTasks_Dev;User Id=sa;Password=d3&j*D1AlC#54jFbo)fw@58lG;");

            return new TmsDbContext(optionsBuilder.Options);
        }
    }
}