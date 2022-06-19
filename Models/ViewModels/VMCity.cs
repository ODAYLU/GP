using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMCity
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string name { get; set; }
       
    }
}
