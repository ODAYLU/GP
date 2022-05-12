using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMAdvertisement
    {
        [DisplayName("كود الاعلان")]
        public long Id { get; set; }
        [Required, DisplayName("الاسم الاول لصاحب الشركة ")]
        public string FirstName { get; set; }
        [Required, DisplayName("الاسم الثاني لصاحب الشركة ")]
        public string LastName { get; set; }
        [Required, DisplayName("رقم هاتف صاحب الشركة ")]
        public double PhoneNumber { get; set; }
        [Required, DisplayName("ايميل الشركة ")]
        public string Email { get; set; }
        [DisplayName(" صورة المعلن عنه ")]
        [Required(ErrorMessage = "الرجاء ادخال الصورة")]
        public string Photo { get; set; }
        [Required, DisplayName("رابط الاعلان ")]
        public string Link { get; set; }
        [Required, DisplayName("السعر ")]
        public double Price { get; set; }
        [Required, DisplayName("تاريخ بداية الاعلان  ")]
        public DateTime StartDate { get; set; }
        [Required, DisplayName("تاريخ نهاية الاعلان  ")]
        public DateTime EndDate { get; set; }
        [DisplayName("وصف الاعلان ")]
        public string Description { get; set; }
        [DisplayName(" صورة المعلن عنه ")]

        [Required(ErrorMessage = "الرجاء ادخال الصورة")]
        public IFormFile image { get; set; }
    }
}
