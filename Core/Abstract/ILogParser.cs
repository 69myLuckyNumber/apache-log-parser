using System.Collections.Generic;
using System.Threading.Tasks;
using ApacheLogParser.Core.Pocos;

namespace ApacheLogParser.Core.Abstract
{
    public interface ILogParser
    {
         Task<IEnumerable<LogEntry>> Parse(string fileName);
    }
}