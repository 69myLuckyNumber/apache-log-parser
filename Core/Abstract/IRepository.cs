using System.Collections.Generic;
using System.Threading.Tasks;
using ApacheLogParser.Core.Models;

namespace ApacheLogParser.Core.Abstract
{
    public interface IRepository
    {
         Task<IEnumerable<Host>> GetAllHostsAsync(bool includeRelated = true);

         Task AddRequestsAsync(IEnumerable<Request> requests); 
    }
}