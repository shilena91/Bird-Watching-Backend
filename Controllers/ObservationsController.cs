using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public ObservationsController(ILogger<ObservationsController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public ActionResult<Observation> CreateNewObservation(int Id)
        {
            try {
                var newObservation = BirdObservationService.Current.AddNewObservationAndLogInfo(Id, _logger);
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