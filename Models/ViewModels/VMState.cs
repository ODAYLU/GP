using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMState
    {

        public long Id { get; set; }
        [Required]
        public string name { get; set; }

        public string ImagePath { get; set; }
        public IFormFile file { get; set; }
     }
}
