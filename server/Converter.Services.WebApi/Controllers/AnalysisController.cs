using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converter.Services.WebApi.Controllers
{
    // http://localhost:24822/Analysis/Start/abc
    [Route("[controller]")]
    public class AnalysisController : Controller
    {
        [HttpPost("[action]/{:id}")]
        public IActionResult Start(string id)
        {
            return Ok();
        }



        [HttpGet("[action]")]
        public IActionResult Retrieve()
        {
            // TODO: implement
            throw new NotImplementedException();
        }

        [HttpGet("[action]/{:id}")]
        public IActionResult Retrieve(string id)
        {
            // TODO: implement
            throw new NotImplementedException();
        }
    }
}
