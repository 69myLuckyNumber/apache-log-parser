using System;

namespace ApacheLogParser.Core.Pocos
{
    public class LogEntry
    {
        public byte[] IPAddressBytes { get; set; }

        public DateTime DateTimeRequested { get; set; }

        public string RequestType { get; set; }

        public int ResponseCode { get; set; }

        public int BytesSent { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string Referer { get; set; }

        public string Browser { get; set; }
    }
}