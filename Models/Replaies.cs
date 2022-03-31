using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class Replaies
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string body { get; set; }
        public long CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comments Comments { get; set; }

    }
}
