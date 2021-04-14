using System;
using bird_watching_backend.Models;
using bird_watching_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bird_watching_backend.Controllers
{
    [ApiController]
    [Route("api/birds/{Id}")]
    public class ObservationsController: ControllerBase
    {
        private readonly ILogger<ObservationsController> _logger;
        private readonly IBirdObservationService _birdObservationservice;

        public ObservationsController(ILogger<ObservationsController> logger, IBirdObservationService service)
        {
            _logger = logger;
            _birdObservationservice = service;
        }

        [HttpPost]
        public ActionResult<Observation> CreateNewObservation(int Id)
        {
            try {
                var newObservation = _birdObservationservice.AddNewObservationAndLogInfo(Id, _logger);
                return Ok(newObservation);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                if (ex is InvalidOperationException)
                {
                    return BadRequest();
                }
                return StatusCode(500);
            }
        }

    }
}