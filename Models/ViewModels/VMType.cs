using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string type { get; set; }
        public string ImagePath { get; set; }

        [Required]
        public IFormFile file { get; set; }

    }
}
