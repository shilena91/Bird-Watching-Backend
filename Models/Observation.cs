using System;

namespace bird_watching_backend.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public string BirdName { get; set; }
        public string createdAt { get; set; }

        public Observation(int Id, string BirdName)
        {
            this.Id = Id;
            this.BirdName = BirdName;
            createdAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public string logging()
        {
            return $"{createdAt} - new observation: {BirdName}";
        }
    }
}
