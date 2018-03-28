using System.Collections.Generic;
using System.Threading.Tasks;
using ApacheLogParser.Core.Models;

namespace ApacheLogParser.Core.Abstract
{
    public interface IRepository
    {
         Task<IEnumerable<Host>> GetAllHosts(bool includeRelated = true);
         
         Task AddRequests(IEnumerable<Request> requests); 
    }
}