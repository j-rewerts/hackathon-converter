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
        public async Task<IActionResult> Start(string id)
        {

            var analysisId = await _repository.AddAnalysisAsync(id);



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
            throw new NotImplementedException();
        }

       
    }
}
