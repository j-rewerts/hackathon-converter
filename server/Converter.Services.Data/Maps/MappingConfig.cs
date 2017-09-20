namespace Converter.Services.Data.Maps
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AnalysisToAnalysisDtoMap.RegisterMap();
            IssueToIssueDtoMap.RegisterMap();
        }
    }
}
