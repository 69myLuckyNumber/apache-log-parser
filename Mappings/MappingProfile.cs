using System.Text;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Models;
using ApacheLogParser.Core.Pocos;
using AutoMapper;

namespace ApacheLogParser.Mappings
{
    public class MappingProfile : Profile
    {
        private readonly IHostParser hostParser;

        public MappingProfile(IHostParser hostParser)
        {
            this.hostParser = hostParser;

            CreateMap<LogEntry, Request>()
                .ForMember(dest => dest.ResponseStatusCode, opts => opts.MapFrom(src => src.ResponseCode))
                .ForMember(dest => dest.RequestType, opts => opts.MapFrom(src => src.RequestType))
                .ForMember(dest => dest.DateTimeRequested, opts => opts.MapFrom(src => src.DateTimeRequested))
                .ForMember(dest => dest.Requestor, opts => opts.MapFrom(src => GetRequestor(src)))
                .ForMember(dest => dest.RequestedFile, opts => opts.MapFrom(src => 
                    new File 
                    { 
                        FileName = src.FileName,
                        FilePath = src.FilePath, 
                        FileSize = src.BytesSent 
                    }));
        }

        private Host GetRequestor(LogEntry log)
        {
            var hostInfo = hostParser.ParseIpAddressAsync(log.IPAddress).Result;
            return new Host 
            { 
                IPAddressBytes = Encoding.ASCII.GetBytes(hostInfo.IP) , 
                HostName = hostInfo.HostName, 
                OrgName = hostInfo.Org 
            };
        }
    }
}