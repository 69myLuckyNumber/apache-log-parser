using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Models;
using ApacheLogParser.Core.Pocos;
using ApacheLogParser.Core.ViewModels;
using AutoMapper;

namespace ApacheLogParser.Mappings
{
    public class MappingProfile : Profile
    {
        private readonly IHostParser hostParser;
        private readonly IRepository repository;

        public MappingProfile(IHostParser hostParser, IRepository repository)
        {
            this.repository = repository;
            this.hostParser = hostParser;

            CreateMap<LogEntry, Request>()
                .ForMember(dest => dest.ResponseStatusCode, opts => opts.MapFrom(src => src.ResponseCode))
                .ForMember(dest => dest.RequestType, opts => opts.MapFrom(src => src.RequestType))
                .ForMember(dest => dest.DateTimeRequested, opts => opts.MapFrom(src => src.DateTimeRequested))
                .ForMember(dest => dest.Requestor, opts => opts.MapFrom(src => GetRequestor(src)))
                .ForMember(dest => dest.RequestorIPAddress, opts => opts.MapFrom(src => Encoding.ASCII.GetBytes(src.IPAddress)))
                .ForMember(dest => dest.RequestedFile, opts => opts.MapFrom(src =>
                    new File
                    {
                        FileName = src.FileName,
                            FilePath = src.FilePath,
                            FileSize = src.BytesSent
                    }));
                

            CreateMap<Request, RequestViewModel>()
                .ForMember(dest => dest.RequestType, opts => opts.MapFrom(src => src.RequestType))
                .ForMember(dest => dest.ResponseStatusCode, opts => opts.MapFrom(src => src.ResponseStatusCode))
                .ForMember(dest => dest.DateTimeRequested, opts => opts.MapFrom(src => src.DateTimeRequested))
                .ForMember(dest => dest.FileName, opts => opts.MapFrom(src => src.RequestedFile.FileName))
                .ForMember(dest => dest.FilePath, opts => opts.MapFrom(src => src.RequestedFile.FilePath))
                .ForMember(dest => dest.BytesSent, opts => opts.MapFrom(src => src.RequestedFile.FileSize));

            CreateMap<Host, HostViewModel>()
                .ForMember(dest => dest.IpAddress, opts => opts.MapFrom(src => Encoding.UTF8.GetString(src.IPAddressBytes)))
                .ForMember(dest => dest.HostName, opts => opts.MapFrom(src => src.HostName))
                .ForMember(dest => dest.OrgName, opts => opts.MapFrom(src => src.OrgName))
                .ForMember(dest => dest.Requests, opts => opts.MapFrom(src => src.Requests));
        }

        private Host GetRequestor(LogEntry log)
        {
            var hostIp = Encoding.ASCII.GetBytes(log.IPAddress);
            if(repository.isHostPresent(hostIp))
                return null;

            var hostInfo = hostParser.ParseIpAddressAsync(log.IPAddress).Result;
            return new Host
            {
                IPAddressBytes = hostIp,
                HostName = hostInfo.HostName,
                OrgName = hostInfo.Org
            };
        }
    }
}