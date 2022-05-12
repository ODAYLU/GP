using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Advertisement
    {
        [Key,DisplayName("كود الاعلان")]
        public long Id{ get; set; }
        [DisplayName("الاسم الاول لصاحب الشركة ")]
        public string FirstName{ get; set; }
        [DisplayName("الاسم الثاني لصاحب الشركة ")]
        public string LastName{ get; set; }
        [DisplayName("رقم هاتف صاحب الشركة ")]
        public double PhoneNumber { get; set; }
        [DisplayName("ايميل الشركة ")]
        public string Email { get; set; }
        [DisplayName("هاتف الشركة ")]
        [Required(ErrorMessage = "الرجاء ادخال الصورة")]
        public string Photo { get; set; }
        [DisplayName("رابط الاعلان ")]
        public string Link { get; set; }
        [DisplayName("السعر ")]
        public double Price { get; set; }
        [DisplayName("تاريخ بداية الاعلان  ")]
        public DateTime StartDate { get; set; }
        [DisplayName("تاريخ نهاية الاعلان  ")]
        public DateTime EndDate { get; set; }
        [DisplayName("وصف الاعلان ")]
        public string Description { get; set; }



    }
}
