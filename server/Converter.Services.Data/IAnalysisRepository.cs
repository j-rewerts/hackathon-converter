using System.Collections.Generic;
using System.Threading.Tasks;

using Converter.Services.Data.DTO;

namespace Converter.Services.Data
{
    public interface IAnalysisRepository
    {
        Task<int> StartAnalysisAsync(string googleFileId);
        Task CompleteAnalysisAsync(int analysisId);
        Task<int> AddCellIssueAsync(int analysisId, int issueTypeId, int cellId, string message);
        Task<int> AddWorkbookIssueAsync(int analysisId, int issueTypeId, string message);
        Task AddWorksheetsAsync(int workbookId, IEnumerable<string> names);
        Task<int> AddCellAsync(string worksheetName, int rowIndex, int columnIndex, string reference, string value, string formula);
        Task UpdateWorksheetCountsAsync(string name, int cellCount, int columnCount, int formulaCount, int rowCount);
        Task<List<AnalysisDto>> RetrieveAnalysisesAsync();
        Task<AnalysisDto> RetrieveAnalysisByGoogleFileIdAsync(string googleFileId);
        Task<WorkbookDto> RetrieveWorkbookByGoogleFileIdAsync(string googleFileId);

        string ConnectionString { get; }
    }
}
