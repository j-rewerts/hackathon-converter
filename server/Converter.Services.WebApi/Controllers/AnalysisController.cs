using Converter.Services.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converter.Services.WebApi.Controllers
{
    /// http://localhost:24822/Analysis/Start/abc123
    /// note: abc123 is the fileId
    [Route("[controller]")]
    public class AnalysisController : Controller
    {
        public AnalysisController(IAnalysisRepository repository)
        {
            this._repository = repository;
        }

        private readonly IAnalysisRepository _repository;

        /// <summary>
        ///  1. get the file
        ///  2. start analyzing the file for given id
        ///  3. if all good then retun Ok(), otherwise throw exception!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{:id}")]
        public IActionResult Start(string id)
        {
            _repository.AddAnalysisAsync(id);

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
        [HttpGet("[action]/{:id}")]
        public IActionResult Retrieve(string id)
        {
            // TODO: implement
            throw new NotImplementedException();
        }

        /// http://localhost:24822/Analysis/Retrieve/abc123
        /// note: 
        [HttpGet("[action]")]
        public IActionResult Retrieve()
        {
            // TODO: implement
            throw new NotImplementedException();
        }

       
    }
}
