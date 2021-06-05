using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Subject")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTimeOffset Created { get; set; }
        [DisplayName("Ticket")]
        public int TicketId { get; set; }
        [Required]
        [DisplayName("Recipient")]
        public string RecipientId { get; set; }
        [Required]
        [DisplayName("Sender")]
        public string SenderId { get; set; }
        public bool Viewed { get; set; }

        //Navigational
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser Recipient { get; set; }
        public virtual BTUser Sender { get; set; }


    }
}
