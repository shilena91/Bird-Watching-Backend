using System;
using System.Collections.Generic;

namespace bird_watching_backend.Models
{
    public class Bird
    {
        public int Id { get; set; }
        public string BirdName { get; set; }

        public int NumberOfObservations
        {
            get
            {
                return Observations.Count;
            }
        }
        
        public List<Observation> Observations { get; set; } = new List<Observation>();

        public Bird(int Id, string BirdName)
        {
            this.Id = Id;
            this.BirdName = BirdName;
        }

        public string logging()
        {
            var pieces = (NumberOfObservations > 1) ? "pieces" : "piece";
            var logInfo = $"{BirdName} {NumberOfObservations} {pieces}";

            return logInfo;
        }
    }
}
