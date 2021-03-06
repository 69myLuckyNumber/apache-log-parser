using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddRequestsAsync(IEnumerable<Request> requests)
        {
            await context.Requests.AddRangeAsync(requests);
        }

        public async Task<IEnumerable<Host>> GetAllHostsAsync(bool includeRelated = true)
        {
            if(!includeRelated)
                return context.Hosts;
            
            return await context.Hosts.Include(h => h.Requests)
                .ThenInclude(r => r.RequestedFile).ToListAsync();
        }

        public bool isHostPresent(byte[] ip)
        {
            return context.Hosts.Any(h => h.IPAddressBytes == ip);
        }

        public bool isRequestPresent(byte[] ip, DateTime time)
        {
            return context.Requests
                .Any(r => r.RequestorIPAddress == ip && r.DateTimeRequested.Equals(time));
        }
    }
}