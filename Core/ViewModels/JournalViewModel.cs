using System.Collections.Generic;

namespace ApacheLogParser.Core.ViewModels
{
    public class JournalViewModel
    {
        public IEnumerable<HostViewModel> Hosts { get; set; }
    }
    public class HostViewModel 
    {
        public string IpAddress { get; set; }

        public string OrgName { get; set; }

        public string HostName { get; set; }

        public IEnumerable<RequestViewModel> Requests { get; set; }

        public HostViewModel()
        {
            Requests = new List<RequestViewModel>();
        }
    }
    public class RequestViewModel 
    {
        public string RequestType { get; set; }

        public string ResponseStatusCode { get; set; }

        public string DateTimeRequested { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public int BytesSent { get; set; }

    }
}