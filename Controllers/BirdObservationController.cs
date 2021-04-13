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
    [Route("api/birds")]
    public class BirdObservationController: ControllerBase
    {
        private readonly ILogger<BirdObservationController> _logger;

        public BirdObservationController(ILogger<BirdObservationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IList<Bird>> GetAllObservations()
        {
            BirdObservationService.Current.readDataFile();
            var birds = BirdObservationService.Current.Birds;
            return Ok(birds);
        }

        [HttpPost]
        public ActionResult<Bird> CreateNewBird([FromBody] BirdForCreation bird)
        {
            try
            {
                var newBird = BirdObservationService.Current.AddNewBirdAndLogInfo(bird, _logger);
                return Ok(newBird);
            }
            catch(Exception error)
            {
                _logger.LogError(error.ToString());
                if (error is InvalidOperationException)
                {
                    ModelState.AddModelError("Description", error.Message);
                    return BadRequest(ModelState);
                }
                return StatusCode(500);
            }
        }
    }
    
}
