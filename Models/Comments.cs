using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class Comments
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(200,ErrorMessage ="لحد 200 حرف ")]
        public string Body { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

    }
}
