using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ApacheLogParser.Core.Models
{
    public class Host
    {
        [Required, MinLength(4), MaxLength(16)]
        public byte[] IPAddressBytes { get; set; }

        public string HostName { get; set; }

        public string OrgName { get; set; }

        public ICollection<Request> Requests { get; set; }

        [NotMapped]
        public IPAddress IPAddress
        {
            get { return new IPAddress(IPAddressBytes); }
            set { IPAddressBytes = value.GetAddressBytes(); }
        }

        public Host()
        {
            Requests = new Collection<Request>();        
        }
    }
}