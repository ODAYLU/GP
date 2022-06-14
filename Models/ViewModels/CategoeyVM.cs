using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Models.ViewModels
{
    public class CategoeyVM
    {
        public int Id { get; set; }
        [Required]
        public string category { get; set; }

        public string ImagePath { set; get; }

        [Required(ErrorMessage = "الرجاء ادخال الصورة")]

        public IFormFile file { get; set; }

    }
}
