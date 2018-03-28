using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;

namespace ApacheLogParser.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}