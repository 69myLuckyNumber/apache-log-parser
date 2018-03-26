using Microsoft.EntityFrameworkCore;

namespace ApacheLogParser.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    }
}