using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data
{
    public static class AnalysisRepositoryExtensions
    {
        public static void AddAnalysisRepository(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<AnalysisContext>(o => options(o));
            services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        }
    }
}
