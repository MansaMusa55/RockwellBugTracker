using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models.ViewModel
{
    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<BTUser> Users { get; set; }
        public string CurrentImage { get; set; }
        public List<Ticket> DevelopementTickets { get; set; }
        public List<Ticket> SubmittedTickets { get; set; }
        public List<Ticket> UnassignedTickets { get; set; }
        public Array[] ChartData { get; set; }
        public MyTicketsViewModel MyTickets { get; set; }
 

    }

}
