using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RockwellBugTracker.Data;
using RockwellBugTracker.Extensions;
using RockwellBugTracker.Models;
using RockwellBugTracker.Models.ViewModel;
using RockwellBugTracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBTCompanyInfoService _infoService;
        private readonly IBTTicketService _ticketService;
        private readonly UserManager<BTUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBTCompanyInfoService infoService, IBTTicketService ticketService, UserManager<BTUser> userManager)
        {
            _logger = logger;
            _context = context;
            _infoService = infoService;
            _ticketService = ticketService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            var projects = await _infoService.GetAllProjectsAsync(companyId);
            var tickets = await _ticketService.GetAllTicketsByCompanyAsync(companyId);
            var viewModel = new DashboardViewModel
            {
                Projects = projects,
                Tickets = tickets,
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
