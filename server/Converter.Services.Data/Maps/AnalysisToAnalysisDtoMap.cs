using AutoMapper;
using Converter.Services.Data.DTO;
using Converter.Services.Data.Models;

namespace Converter.Services.Data.Maps
{
    public static class AnalysisToAnalysisDtoMap
    {
        public static void RegisterMap()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Analysis, AnalysisDto>()
                .ForMember(d => d.AnalysisID, opt => opt.MapFrom(c => c.AnalysisID))
                .ForMember(d => d.AnalysisStatus, opt => opt.MapFrom(c => c.AnalysisStatus))
                .ForMember(d => d.WorkbookName, opt => opt.MapFrom(c => c.Workbook.Name))
                .ForMember(d => d.Issues, opt => opt.MapFrom(c => c.Issues)));
        }
    }
}
