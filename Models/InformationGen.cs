using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Models
{
    public class InformationGen
    {

        public int Id { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Phone,Required]
        public string Phone { get; set; }
        [Url]
        public string UrlFacebook { get; set; }
        [Url]
        public string UrlTwitter { get; set; }
        [Url]
        public string UrlInstegrame { get; set; }
    }
}
