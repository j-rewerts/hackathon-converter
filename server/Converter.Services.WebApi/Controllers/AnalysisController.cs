using Converter.Services.Data;
using Converter.Services.OpenXml;
using Converter.Services.TaskRunner;
using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Converter.Services.WebApi.Controllers
{
    /// http://localhost:24822/Analysis/Start/abc123
    /// note: abc123 is the fileId
    [Route("[controller]")]
    public class AnalysisController : Controller
    {
        public AnalysisController(IHostingEnvironment env,
            IAnalysisRepository repository,
            ExcelAnalyzer excelAnalyzer,
            ILogger<AnalysisController> logger)
        {
            _env = env;
            _repository = repository;
            _excelAnalyzer = excelAnalyzer;
            _logger = logger;
        }

        private readonly IHostingEnvironment _env;
        private readonly IAnalysisRepository _repository;
        private readonly ExcelAnalyzer _excelAnalyzer;
        private readonly ILogger<AnalysisController> _logger;

        /// <summary>
        ///  1. get the file
        ///  2. start analyzing the file for given id
        ///  3. if all good then retun Ok(), otherwise throw exception!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Start(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            string oauthToken = OAuthToken;
            if (string.IsNullOrWhiteSpace(oauthToken))
                throw new SecurityException("No OAuth token provided in request");

            //Task<int> analysisIdTask;
            int analysisId;
            try
            {
                analysisId =  await _repository.StartAnalysisAsync(id);
            }
            catch (Exception err)
            {
                // TODO: create proper EventIds for logging
                _logger?.LogError(0, err, "Unable to save new analysis to database");
                throw err;
            }

            // start analyzing immediately on new thread
            ThreadPool.QueueUserWorkItem(async s =>
            {
                // Can't use Dependency Injection because our calling thread will
                // dispose the objects
                var excelAnalyzer = new ExcelAnalyzer(
                    AnalysisRepositoryFactory.CreateRepository(), _excelAnalyzer.Logger);

                await excelAnalyzer.AnalyzeAsync(id, oauthToken);
            });            

            return Ok();
        }




        /// <summary>
        /// return analysis
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// {
        ///     status:"....","not started","In Porgress", "Completed", "Failed"
        ///     Issues:[{Message:"the message"}]
        /// }</returns>
        [HttpGet("{:id}")]
        public IActionResult Retrieve(string id)
        {
            // TODO: implement
            //throw new NotImplementedException();
            // example return value
            return Ok(new
            {
                status =  "Completed",
                issues =  new object[] { new { message = "Contains VBA" } },
                sheets = new object[]  
                { 
                    new {
                        cellCount = 1000000,
                        rowCount = 1000,
                        name = "Sheet1",
                        issues = new object[]
                        {
                            new {
                                message = "Too many cells"
                            }, new {
                                message = "Many cells have formula"
                            }
                        }
                    },
                    new {
                        cellCount = 86,
                        rowCount = 43,
                        name = "Sheet2",
                        issues = new object[]
                        { }
                    }
                }
            });
        }

        /// http://localhost:24822/Analysis/Retrieve/abc123
        /// note: 
        [HttpGet()]
        public IActionResult Retrieve()
        {
            // TODO: implement
            return Ok(new
            {
                tasks = new object[] {
                    new {
                        id = "1",
                        status = "Completed",
                        filename = "Example File 1.xlsx",
                        issues = new object[] {
                            new { id = "1", type = "hasVBA", message = "This workbook contains VBA macros." },
                            new { id = "2", type = "tooManyCells", message = "Sheet 'Sheet1' has over 2000000 cells." },
                            new { id = "3", type = "hasVBA", message = "Sheet 'Sheet1' has too many formulas." }
                        },
                    },
                    new {
                        id = "2",
                        status = "Completed",
                        filename = "Example File 2.xlsx",
                        issues = new object[] {},
                    },
                    new {
                        id = "3",
                        status = "In progress",
                        filename = "Example File 3.xlsx",
                        issues = new object[] {
                            new { id = "4", type = "hasVBA", message = "This workbook contains VBA macros." },
                        },
                    },
                    new {
                        id = "4",
                        status = "In progress",
                        filename = "Example File 3.xlsx",
                        issues = new object[] {
                            new { id = "4", type = "hasVBA", message = "This workbook contains VBA macros." },
                        },
                    },
                    new {
                        id = "5",
                        status = "In progress",
                        filename = "Not an Excel file.docx",
                        issues = new object[] {
                            new { id = "5", type = "embeddedImage", message = "This document contains embedded images." },
                        },
                    }
                }

        });
        }

        protected string OAuthToken
        {
            get
            {
                var authToken = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(authToken) &&
                    authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) &&
                    authToken.Length > 7)
                    return authToken.Substring(7);
                else
                    return null;
            }
        }
       
    }
}
