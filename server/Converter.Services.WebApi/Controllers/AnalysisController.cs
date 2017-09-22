using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Converter.Services.Data;
using Converter.Services.Data.DTO;
using Converter.Services.TaskRunner;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> Retrieve(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            AnalysisDto analysis = new AnalysisDto();
            try
            {
                analysis = await _repository.RetrieveAnalysisByGoogleFileIdAsync(id);
            }
            catch (Exception err)
            {
                // TODO: create proper EventIds for logging
                _logger?.LogError(0, err, "Unable to retrieve analysis from database");
                throw err;
            }
            return Ok(analysis);
        }

        /// http://localhost:24822/Analysis/Retrieve/abc123
        /// note: 
        [HttpGet()]
        public async Task<IActionResult> Retrieve()
        {
            List<AnalysisDto> analysises = new List<AnalysisDto>();
            try
            {
                analysises = await _repository.RetrieveAnalysisesAsync();
            }
            catch (Exception err)
            {
                // TODO: create proper EventIds for logging
                _logger?.LogError(0, err, "Unable to retrieve analysises from database");
                throw err;
            }
            return Ok(analysises);
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
