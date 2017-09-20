using System.Threading.Tasks;

namespace Converter.Services.Data
{
    public interface IAnalysisRepository
    {
        Task<int> StartAnalysisAsync(string googleFileId);
        Task<int> CompleteAnalysisAsync(int analysisId);
        Task<int> AddCellIssueAsync(int analysisId, int issueTypeId, int cellId, string message);
        Task<int> AddWorkbookIssueAsync(int analysisId, int issueTypeId, string message);
        Task<int> AddWorksheetAsync(int workbookId, string name, int rowCount, int cellCount);
    }
}
