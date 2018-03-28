using System.Net.Http;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Pocos;
using Newtonsoft.Json;

namespace ApacheLogParser.Tools
{
    public class HostParser : IHostParser
    {
        private readonly HttpClient httpClient;

        public HostParser(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HostInfo> ParseIpAddressAsync(string ip)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://ipinfo.io/{ip}/json/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                var hostInfo = JsonConvert.DeserializeObject<HostInfo>(responseBody);

                return hostInfo;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}