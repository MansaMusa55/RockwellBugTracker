using Microsoft.EntityFrameworkCore;
using RockwellBugTracker.Data;
using RockwellBugTracker.Models;
using RockwellBugTracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Services
{
    public class BTNotificationService : IBTNotificationService

       
    {   
        private readonly ApplicationDbContext _context;
        private readonly IBTEmailSender _emailSender;
        private readonly IBTCompanyInfoService _infoService;


        public BTNotificationService(ApplicationDbContext context, IBTEmailSender emailSender, IBTCompanyInfoService infoService)
        {
            _context = context;
            _emailSender = emailSender;
            _infoService = infoService;
        }

        public async Task AdminsNotificationAsync(Notification notification, int companyId)
        {
            try
            {
                //Get company admin(s)
                List<BTUser> admins = await _infoService.GetMembersInRoleAsync("Admin", companyId);

                foreach (BTUser btUser in admins)
                {
                    notification.RecipientId = btUser.Id;

                    await EmailNotificationAsync(notification, notification.Title);

                }
            }
            catch
            {
                throw;
            }
        }

        public async Task EmailNotificationAsync(Notification notification, string emailSubject)
        {
            BTUser btUser = await _context.Users.FindAsync(notification.RecipientId);

            //Send Email
            string btUserEmail = btUser.Email;
            string message = notification.Message;

            try
            {
                await _emailSender.SendEmailAsync(btUserEmail, emailSubject, message);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            List<Notification> notifications = await _context.Notification
                                                       .Include(n => n.Recipient)
                                                       .Include(n => n.Sender)
                                                       .Include(n => n.Ticket)
                                                            .ThenInclude(t => t.Project)
                                                        .Where(n => n.RecipientId == userId).ToListAsync();

            return notifications;
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            List<Notification> notifications = await _context.Notification
                                           .Include(n => n.Recipient)
                                           .Include(n => n.Sender)
                                           .Include(n => n.Ticket)
                                                .ThenInclude(t => t.Project)
                                            .Where(n => n.SenderId == userId).ToListAsync();

            return notifications;

        }

        public async Task MembersNotificationAsync(Notification notification, List<BTUser> members)
        {
            try
            {
                foreach (BTUser btUser in members)
                {
                    notification.RecipientId = btUser.Id;

                    await EmailNotificationAsync(notification, notification.Title);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task SaveNotificationAsync(Notification notification)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public Task SMSNotificationAsync(string phone, Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
