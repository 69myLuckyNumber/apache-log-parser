namespace ApacheLogParser.Core.Models
{
    public class File
    {
        public int FileId { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public int FileSize { get; set; }  

        public int RequestId { get; set; } 
        public Request Request { get; set; }
    }
}