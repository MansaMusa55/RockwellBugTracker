using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models
{
    public class TicketPriority
    {
        //Primary Key
        public int Id { get; set; }
        [DisplayName("Ticket Priority")]
        public string Name { get; set; }
    }
}
