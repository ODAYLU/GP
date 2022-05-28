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
        [Required(ErrorMessage ="هذا الحقل مطلوب ")]
        [EmailAddress(ErrorMessage ="يرجى ادخال ايميل بشكل صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        [EmailAddress(ErrorMessage = "يرجى ادخال الرقم بشكل صحيح")]
        public string Phone { get; set; }
        [Url(ErrorMessage ="يجب أن يكون رابط صحيح")]
        public string UrlFacebook { get; set; }
        [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
        public string UrlTwitter { get; set; }
        [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
        public string UrlInstegrame { get; set; }
    }
}
