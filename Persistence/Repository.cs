using System.Collections.Generic;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ApacheLogParser.Persistence
{
    public class Repository : IRepository
    {
        private readonly AppDbContext context;

        public Repository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddRequests(IEnumerable<Request> requests)
        {
            await context.Requests.AddRangeAsync(requests);
        }

        public async Task<IEnumerable<Host>> GetAllHosts(bool includeRelated = true)
        {
            if(!includeRelated)
                return context.Hosts;
            
            return await context.Hosts.Include(h => h.Requests)
                .ThenInclude(r => r.RequestedFile).ToListAsync();
        }
    }
}