using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GP.Models  
{

    public class AppUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [DataType(dataType: DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [DataType(dataType:DataType.ImageUrl)]
        public string ImagePath { get; set; }
        public bool is_active { get; set; }
        public int memory { get; set; }
        public bool is_special { get; set; }

        public string Role_User { get; set; }
        public string decription { get; set; }
        

    }
}
