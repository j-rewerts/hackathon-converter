using Converter.Services.Data;
using Converter.Services.TaskRunner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Converter.Services.Data.DTO;


namespace Converter.Services.WebApi.Controllers
{
    /// http://localhost:24822/Analysis/Start/abc123
    /// note: abc123 is the fileId
    [Route("[controller]")]
    public class AnalysisController : Controller, IDisposable
    {
        public AnalysisController(IHostingEnvironment env,
            /*IAnalysisRepository repository,*/
            /*ExcelAnalyzer excelAnalyzer,*/
            ILogger<AnalysisController> logger)
        {
            _env = env;
            _logger = logger;
            //_repository = repository;
            // skipping the dependency injection because it seems to be broken in the Google Cloud
            _logger?.LogInformation("Starting AnalysisController");
            _repository = AnalysisRepositoryFactory.CreateRepository(GetDbContextOptions());
            //_excelAnalyzer = excelAnalyzer;
            _excelAnalyzer = new ExcelAnalyzer(_repository);
        }

        private Microsoft.EntityFrameworkCore.DbContextOptions GetDbContextOptions()
        {

            var b = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder();
            b.UseMySql("server=35.197.80.226;Database=Converter;Uid=root;Pwd=P@ssw0rd");
            return b.Options;
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
            {
                throw new ArgumentNullException("id");
            }

            string oauthToken = OAuthToken;
            if (string.IsNullOrWhiteSpace(oauthToken))
            {
                _logger.LogWarning($"Analysis requested but no OAuth token was provided. File id was { id }");
                throw new SecurityException("No OAuth token provided in request");
            }

            int analysisId;
            try
            {
                _logger.LogInformation($"Adding Google File to database for starting anaylsis: { id }");
                _logger.LogInformation($"Using connection string: { _repository.ConnectionString }");
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
                _logger.LogInformation("Starting analysis for Google file { id }");

                // Can't use Dependency Injection because our calling thread will
                // dispose the objects
                var excelAnalyzer = new ExcelAnalyzer(
                    AnalysisRepositoryFactory.CreateRepository(GetDbContextOptions()));

                await excelAnalyzer.AnalyzeAsync(analysisId, id, oauthToken);
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
                //try
                //{
                //    Data.Maps.MappingConfig.RegisterMaps();
                //}
                //catch (Exception) { }
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            var repository = _repository as IDisposable;
            if (repository != null)
                repository.Dispose();
        }

    }
}
