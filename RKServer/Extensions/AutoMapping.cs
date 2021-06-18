using AutoMapper;
using Server.Models.Models;
using Server.Models.Request;
using Server.Models.Response;
using System.Collections.Generic;

namespace RKServer.Extensions
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ComputerResponse, Computer>().ReverseMap();
            CreateMap<ComputerRequest, Computer>().ReverseMap();
            CreateMap<IEnumerable<ComputerResponse>, IEnumerable<Computer>>();
        }
    }
}
