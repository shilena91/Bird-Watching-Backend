using System;
using System.Collections.Generic;
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
        private readonly IBirdObservationService _birdObservationservice;

        public BirdObservationController(ILogger<BirdObservationController> logger, IBirdObservationService service)
        {
            _logger = logger;
            _birdObservationservice = service;
        }

        [HttpGet]
        public ActionResult<IList<Bird>> GetAllObservations()
        {
            _birdObservationservice.readDataFile();
            var birds = _birdObservationservice.GetBirds();
            return Ok(birds);
        }

        [HttpPost]
        public ActionResult<Bird> CreateNewBird([FromBody] BirdForCreation bird)
        {
            try
            {
                var newBird = _birdObservationservice.AddNewBirdAndLogInfo(bird, _logger);
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
