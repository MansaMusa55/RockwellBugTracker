using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Services.Interfaces
{
    public interface IBTEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
