using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Models.ViewModels
{
    public class ContactVM
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public string Object { get; set; }
        [Required]
        public string Description { get; set; }
        public string Msg { get; set; }
    }
}
