using System;

namespace ApacheLogParser.Core.Pocos
{
    public class LogEntry
    {
        public string IPAddress { get; set; }

        public DateTime DateTimeRequested { get; set; }

        public string RequestType { get; set; }

        public int ResponseCode { get; set; }

        public int BytesSent { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }
    }
}