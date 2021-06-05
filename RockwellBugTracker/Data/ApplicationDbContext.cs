using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RockwellBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RockwellBugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RockwellBugTracker.Models.Company> Company { get; set; }
        public DbSet<RockwellBugTracker.Models.Invite> Invite { get; set; }
        public DbSet<RockwellBugTracker.Models.Notification> Notification { get; set; }
        public DbSet<RockwellBugTracker.Models.Project> Project { get; set; }
        public DbSet<RockwellBugTracker.Models.ProjectPriority> ProjectPriority { get; set; }
        public DbSet<RockwellBugTracker.Models.Ticket> Ticket { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketAttachment> TicketAttachment { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketComment> TicketComment { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketHistory> TicketHistory { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketPriority> TicketPriority { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketStatus> TicketStatus { get; set; }
        public DbSet<RockwellBugTracker.Models.TicketType> TicketType { get; set; }
    }
}
