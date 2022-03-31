using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMCurrency
    {
        [Required]
        public int Id { set; get; }
        [Required]
        public string currency { get; set; }
    }
}
