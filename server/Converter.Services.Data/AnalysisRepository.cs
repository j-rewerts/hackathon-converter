using Converter.Services.Data.Models;
using System.Threading.Tasks;

namespace Converter.Services.Data
{
    internal class AnalysisRepository : IAnalysisRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>context is typically provided through dependency injection</remarks>
        public AnalysisRepository(AnalysisContext context)
        {
            this._context = context;
        }

        private readonly AnalysisContext _context;

        public async Task<int> AddAnalysisAsync(string fileId)
        {
            var analysis = new Analysis();
            // TODO; set workbook properties here (maybe just fileId?)
            _context.Analysis.Add(analysis);
            await _context.SaveChangesAsync();

            return analysis.AnalysisID;
        }
    }
}
