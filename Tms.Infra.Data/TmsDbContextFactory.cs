using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tms.Infra.Data
{
    /// <summary>
    /// Factory to run Migrations on command line
    /// </summary>
    public class TmsDbContextFactory : IDesignTimeDbContextFactory<TmsDbContext>
    {
        public TmsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TmsDbContext>();
            optionsBuilder.UseSqlServer("STRING_CONNECTION");

            return new TmsDbContext(optionsBuilder.Options);
        }
    }
}