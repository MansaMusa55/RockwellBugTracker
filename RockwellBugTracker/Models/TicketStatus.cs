using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models
{
    public class TicketStatus
    {
        //Primary Key
        public int Id { get; set; }
        [DisplayName("Ticket Status")]
        public string Name { get; set; }
    }
}
