using AutoMapper;
using Converter.Services.Data.DTO;
using Converter.Services.Data.Models;

namespace Converter.Services.Data.Maps
{
    public static class IssueToIssueDtoMap
    {
        public static void RegisterMap()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<IssueBase, IssueDto>()
                .ForMember(d => d.IssueID, opt => opt.MapFrom(c => c.IssueID))
                .ForMember(d => d.IssueType, opt => opt.MapFrom(c => c.IssueType))
                .ForMember(d => d.Message, opt => opt.MapFrom(c => c.Message)));
        }
    }
}
