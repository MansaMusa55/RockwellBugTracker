using Microsoft.AspNetCore.Authorization;
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
        private readonly IBTProjectService _projectService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBTCompanyInfoService infoService, IBTTicketService ticketService, UserManager<BTUser> userManager, IBTProjectService projectService)
        {
            _logger = logger;
            _context = context;
            _infoService = infoService;
            _ticketService = ticketService;
            _userManager = userManager;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            List<Project> projects = await _projectService.GetAllProjectsByCompany(companyId);

            int urgent = (await _ticketService.LookupTicketPriorityIdAsync("Urgent")).Value;
            int high = (await _ticketService.LookupTicketPriorityIdAsync("High")).Value;
            int medium = (await _ticketService.LookupTicketPriorityIdAsync("Medium")).Value;
            int low = (await _ticketService.LookupTicketPriorityIdAsync("Low")).Value;

            List<Array> data = new();

            data.Add(new string[] { "Project", "Urgent", "High", "Medium", "Low" });
            foreach (Project project in projects)
            {
                data.Add(new object[] { project.Name,
                                                    project.Tickets.Where(t => t.TicketStatusId == urgent).Count(),
                                                    project.Tickets.Where(t => t.TicketStatusId == high).Count(),
                                                    project.Tickets.Where(t => t.TicketStatusId == medium).Count(),
                                                    project.Tickets.Where(t => t.TicketStatusId == low).Count()
                });
            }

            DashboardViewModel model = new()
            {
                Projects = await _projectService.GetAllProjectsByCompany(companyId),
                Tickets = await _ticketService.GetAllTicketsByCompanyAsync(companyId),
                Users = await _infoService.GetAllMembersAsync(companyId),
                ChartData = data.ToArray()
            };

            return View(model);
        }



        [HttpPost]
        public async Task<JsonResult> PieChartMethod()
        {
            int companyId = User.Identity.GetCompanyId().Value;

            List<Project> projects = await _projectService.GetAllProjectsByCompany(companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "ProjectName", "TicketCount" });

            foreach (Project prj in projects)
            {
                chartData.Add(new object[] { prj.Name, prj.Tickets.Count() });
            }

            return Json(chartData);
        }

        public IActionResult Landing()
        {
            return View();
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
