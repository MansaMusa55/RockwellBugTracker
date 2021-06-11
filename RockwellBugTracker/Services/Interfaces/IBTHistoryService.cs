using RockwellBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Services.Interfaces
{
    public interface IBTHistoryService
    {
        Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId);
        Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int companyId);
        Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId);
    }
}
