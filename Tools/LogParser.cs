using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Pocos;

namespace ApacheLogParser.Tools
{
    public class LogParser : ILogParser
    {
        public async Task<IEnumerable<LogEntry>> Parse(string fileName)
        {
            var regex = new Regex("^([\\d.]+) (\\S+) (\\S+) \\[([\\w:/]+\\s[+\\-]\\d{4})\\] \"(.+?)\" (\\d{3}) (\\d+) \"([^\"]+)\" \"([^\"]+)\"");
           
            var logEntries = (await File.ReadAllLinesAsync(fileName))
                .Select(str =>
                {
                    var m = regex.Match(str);
                    if (m.Success && m.Groups[5].Value.Contains(".html HTTP"))
                    {
                        var reqLogStr = m.Groups[5].Value;

                        var reqType = reqLogStr.Substring(0, reqLogStr.IndexOf("/")-1);
                        var reqfilePath = reqLogStr.Substring(reqType.Length + 1, reqLogStr.LastIndexOf(".html") + 1);
                        var reqfileName = reqfilePath.Substring(reqfilePath.LastIndexOf("/")+1);

                        var dateStr = m.Groups[4].Value.Substring(0, m.Groups[4].Value.IndexOf("-") - 1);
                        var datetime = DateTime.ParseExact(dateStr, "dd/MMM/yyyy:H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        return new LogEntry
                        {
                            IPAddress = m.Groups[1].Value,
                            DateTimeRequested = datetime,
                            RequestType = reqType,
                            FileName = reqfileName,
                            FilePath = reqfilePath,
                            ResponseCode = Int32.Parse(m.Groups[6].Value),
                            BytesSent = Int32.Parse(m.Groups[7].Value)
                        };
                    }
                    return null;
                }).Where(entry => entry != null);

            return logEntries;
        }
    }
}
