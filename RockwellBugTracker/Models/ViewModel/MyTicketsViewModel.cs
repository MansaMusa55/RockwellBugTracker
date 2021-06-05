using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models.ViewModel
{
    public class MyTicketsViewModel
    {
        public IEnumerable<Ticket> DeveloperTickets { get; set; }
        public IEnumerable<Ticket> SubmittedTickets { get; set; }

    }
}
