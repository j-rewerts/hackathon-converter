using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Converter.Services.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // based on https://stackoverflow.com/questions/42414397/asp-net-core-mvc-catch-all-route-serve-static-file
        public IActionResult Spa()
        {
            return File("~/index.html", "text/html");
        }
    }
}
