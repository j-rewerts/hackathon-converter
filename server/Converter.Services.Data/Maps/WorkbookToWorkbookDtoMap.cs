using AutoMapper;
using Converter.Services.Data.DTO;
using Converter.Services.Data.Models;

namespace Converter.Services.Data.Maps
{
    public static class WorkbookToWorkbookDtoMap
    {
        public static void RegisterMap()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Workbook, WorkbookDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(c => c.WorkbookID))
                .ForMember(d => d.GoogleFileId, opt => opt.MapFrom(c => c.GoogleFileID))
                .ForMember(d => d.Name, opt => opt.MapFrom(c => c.Name)));
        }
    }
}
