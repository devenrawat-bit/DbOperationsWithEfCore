using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEfCoreApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
