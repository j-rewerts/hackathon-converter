using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Converter.Services.TaskRunner
{
    public class AnalysisRunner
    {
        public void AnalylzeExcelFile(string googleId)
        {
            // simulate a long running process for now
            Thread.Sleep(new TimeSpan(0, 2, 0));
        }
    }
}
