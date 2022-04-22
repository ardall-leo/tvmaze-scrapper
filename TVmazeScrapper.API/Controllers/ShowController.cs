using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVmazeScrapper.Infrastructure.Persistences;

namespace TVmazeScrapper.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ShowController> _logger;
        private readonly ShowRepository _showRepository;

        public ShowController(ILogger<ShowController> logger, ShowRepository showRepository)
        {
            _logger = logger;
            _showRepository = showRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_showRepository.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
