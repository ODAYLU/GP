using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMState
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string name { get; set; }

        public string ImagePath { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال الصورة")]

        public IFormFile file { get; set; }
     }
}
