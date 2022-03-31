using System.ComponentModel.DataAnnotations;

namespace GP.Models.ViewModels
{
    public class VMCity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
       
    }
}
