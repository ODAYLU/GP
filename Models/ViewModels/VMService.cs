using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMService
    {
         [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "نوع الخدمة")]
        public string Name { get; set; }

        public string ImagePath { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال الصورة")]

        public IFormFile file { get; set; }
    }
}
