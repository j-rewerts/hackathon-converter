using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data
{
    public static class AnalysisRepositoryFactory
    {
        public static IAnalysisRepository CreateRepository()
        {
            return new AnalysisRepository(
                new AnalysisContext());
        }

        //public static IAnalysisRepository CreateRepository(DbContextOptions<AnalysisContext> options)
        //{
        //    return new AnalysisRepository(
        //        new AnalysisContext(options));
        //}
    }
}
