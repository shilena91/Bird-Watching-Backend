using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bird_watching_backend.Controllers;
using bird_watching_backend.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace bird_watching_backend.Services
{
    public interface IBirdObservationService {
        List<Bird> GetBirds();
        void readDataFile();
        Observation AddNewObservationAndLogInfo(int id, ILogger<ObservationsController> _logger);
        Bird AddNewBirdAndLogInfo(BirdForCreation bird, ILogger<BirdObservationController> _logger);
    }

    public class BirdObservationService: IBirdObservationService
    {
        private string path = "./Data/data.json";
        
        private List<Bird> Birds { get; set; } = new List<Bird>();

        public List<Bird> GetBirds()
        {
            return Birds;
        }

        public void readDataFile()
        {
            if (!File.Exists(path))
            {
                var newBirds = new List<Bird>()
                                    {
                                        new Bird(Id: 1, BirdName: "The Crow"),
                                        new Bird(Id: 2, BirdName: "Magpie")
                                    };
                var jsonResult = JsonConvert.SerializeObject(newBirds, Formatting.Indented);
                File.WriteAllText(path, jsonResult);
            }
            var jsonText = File.ReadAllText(path);
            Birds = JsonConvert.DeserializeObject<List<Bird>>(jsonText);     
        }

        public Observation AddNewObservationAndLogInfo(int id, ILogger<ObservationsController> _logger)
        {
            var bird = Birds.FirstOrDefault(b => b.Id == id);

            if (bird == null)
            {
                throw new InvalidOperationException("Bad request, wrong bird's id");
            }
            
            // for only this assignment purpose, to be improved when working with real database
            var observations = Birds.SelectMany(b => b.Observations);
            var maxObservationId = 0;

            if (observations.Count() > 0)
            {
                maxObservationId = observations.Max(o => o.Id);
            }

            var newObservationId = ++maxObservationId;
            var newObservation = new Observation(Id: newObservationId, BirdName: bird.BirdName);
            var birdNeeded = Birds.FirstOrDefault(b => b.BirdName == newObservation.BirdName);

            if (birdNeeded ==  null)
            {
                throw new InvalidDataException("Something wrong with data file");
            }

            birdNeeded.Observations.Add(newObservation);
            var jsonData = JsonConvert.SerializeObject(Birds, Formatting.Indented);
            File.WriteAllText(path, jsonData);

            logNewObservationInfo(newObservation, _logger);

            return newObservation;
        }

        public Bird AddNewBirdAndLogInfo(BirdForCreation bird, ILogger<BirdObservationController> _logger)
        {
            var duplicatedBirdName = Birds.FirstOrDefault(b => b.BirdName == bird.BirdName);
            if (duplicatedBirdName != null)
            {
                throw new InvalidOperationException("This bird is already in the list.");
            }

            var maxBirdId = Birds.Max(b => b.Id);
            var newId = ++maxBirdId;
            
            var newBird = new Bird(newId, bird.BirdName);
            Birds.Add(newBird);

            var jsonData = JsonConvert.SerializeObject(Birds, Formatting.Indented);
            File.WriteAllText(path, jsonData);
            logNewBirdInfo(newBird, _logger);
            return newBird;
        }

        private void logNewObservationInfo(Observation newObservation, ILogger<ObservationsController> _logger)
        {
            var logInfo = $"{newObservation.logging()} - all observations: ";
            var birdsExceptLast = Birds.SkipLast(1);
            foreach (var bird in birdsExceptLast)
            {
                logInfo += $"{bird.logging()}, ";
            }
            var lastBird = Birds.Last();
            logInfo += $"{lastBird.logging()}.";

            _logger.LogInformation(logInfo);
        }

        private void logNewBirdInfo(Bird newBird, ILogger<BirdObservationController> _logger)
        {
            var timeStamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var logInfo = $"{timeStamp} - species addition: {newBird.BirdName}";

            _logger.LogInformation(logInfo);
        }

    }
}