using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TVmazeScrapper.Infrastructure.Persistences;

namespace TVmazeScrapper.API.Controllers
{
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly ILogger<ShowController> _logger;
        private readonly UnitOfWork _unitOfWork;

        public ShowController(ILogger<ShowController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("shows")]
        public IActionResult Get(int offset, int pageSize)
        {
            try
            {
                return Ok(_unitOfWork.GetShows(offset, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("show/{id}")]
        public IActionResult Get(long? id)
        {
            try
            {
                return Ok(_unitOfWork.GetShow(id));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
