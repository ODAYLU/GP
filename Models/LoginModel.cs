using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class LoginData
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
