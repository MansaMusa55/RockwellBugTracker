using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models.ViewModel
{
    public class MyTicketsViewModel
    {
        public List<Ticket> DeveloperTickets { get; set; }
        public List<Ticket> SubmittedTickets { get; set; }

    }
}
