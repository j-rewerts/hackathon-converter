using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Services.Data
{
    public interface IAnalysisRepository
    {
        Task<int> AddAnalysisAsync(string fileId);

        // TODO: add methods for retrieving analysis results

        // We should probably make the Entity Framework classes internal
        // and create new public classes for returning the values.
        // (prevents leaky abstractions).
        // We can use Automapper to map between the types.
    }
}
