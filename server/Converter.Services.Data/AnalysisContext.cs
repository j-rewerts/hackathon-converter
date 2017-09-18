using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.Data
{
    public class AnalysisContext : DbContext
    {
        public AnalysisContext() : base() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO: setup fluent API stuff 
        }
    }
}
