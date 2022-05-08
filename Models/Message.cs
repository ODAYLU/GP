using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Models
{
    public class Message
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
       
        public DateTime Time { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser Sender { get; set; }

        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual AppUser Receiver { get; set; }
    }
}
