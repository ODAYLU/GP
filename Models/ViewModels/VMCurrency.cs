using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMCurrency
    {
        [Required]
        public int Id { set; get; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string currency { get; set; }
    }
}
