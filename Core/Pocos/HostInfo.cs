using Newtonsoft.Json;

namespace ApacheLogParser.Core.Pocos
{
    public class HostInfo
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }
    }
}