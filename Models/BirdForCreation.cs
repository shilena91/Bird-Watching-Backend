using System.ComponentModel.DataAnnotations;

namespace bird_watching_backend.Models
{
    public class BirdForCreation
    {
        [Required(ErrorMessage = "A name for new bird is required.")]
        [MaxLength(40)]
        public string BirdName { get; set; }
    }
}
