using System.Threading.Tasks;
using ApacheLogParser.Core.Pocos;

namespace ApacheLogParser.Core.Abstract
{
    public interface IHostParser
    {
        Task<HostInfo> ParseIpAddressAsync(string ip);
    }
}