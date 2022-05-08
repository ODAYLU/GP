using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class Concrant
    {






        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Users { get; set; }
    }
}
