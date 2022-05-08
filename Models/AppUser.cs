using GP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models  
{

    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Messages = new HashSet<Message>();
        }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [DataType(dataType: DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [DataType(dataType:DataType.ImageUrl)]
        public string ImagePath { get; set; }
        public bool is_active { get; set; } = true;
        public int memory { get; set; }
        public bool is_special { get; set; }
        public string decription { get; set; }
        public int NumberLikes { get; set; }
        public string NameRole { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
  
}
