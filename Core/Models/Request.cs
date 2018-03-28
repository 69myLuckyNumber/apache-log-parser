using System;

namespace ApacheLogParser.Core.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        public string RequestType { get; set; }

        public int ResponseStatusCode { get; set; }

        public DateTime DateTimeRequested { get; set; }

        public File RequestedFile { get; set; }

        public Host Requestor { get; set; }
        public byte[] RequestorIPAddress { get; set; }
    }
}