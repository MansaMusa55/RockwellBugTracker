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
using System.Drawing;
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
        private readonly IBTImageService _imageService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBTCompanyInfoService infoService, IBTTicketService ticketService, UserManager<BTUser> userManager, IBTProjectService projectService, IBTImageService imageService)
        {
            _logger = logger;
            _context = context;
            _infoService = infoService;
            _ticketService = ticketService;
            _userManager = userManager;
            _projectService = projectService;
            _imageService = imageService;
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
            string userId = _userManager.GetUserId(User);
            BTUser btUser = await _userManager.GetUserAsync(User);

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
                CurrentImage = _imageService.DecodeImage(btUser.AvatarFileData, btUser.AvatarContentType),
                ChartData = data.ToArray(),
                DevelopementTickets = await _ticketService.GetAllTicketsByRoleAsync("Developer", userId),
                SubmittedTickets = await _ticketService.GetAllTicketsByRoleAsync("Submitter", userId),
                UnassignedTickets = await _ticketService.GetAllUnassignedTicketsAsync(companyId),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> ProjTypeChartMethod()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            List<Ticket> tickets = await _ticketService.GetAllTicketsByCompanyAsync(companyId);
            var types = _context.TicketType.ToList();
            DonutViewModel chartData = new();
            chartData.labels = types.Select(t => t.Name).ToArray();
            Random rnd = new();

            List<SubData> dsArray = new();
            List<int> howManyTickets = new();
            List<string> colors = new();
            //Antonio's Random Colors
            foreach (TicketType type in types)
            {
                howManyTickets.Add(tickets.Where(t => t.TicketTypeId == type.Id).Count());

                // This code will randomly select a color for each element of the data 
                Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                string colorHex = string.Format("#{0:X6}", randomColor.ToArgb() & 0X00FFFFFF);

                colors.Add(colorHex);
            }

            SubData temp = new()
            {
                data = howManyTickets.ToArray(),
                backgroundColor = colors.ToArray()
            };
            dsArray.Add(temp);

            chartData.datasets = dsArray.ToArray();

            return Json(chartData);
        }

        [HttpPost]
        public async Task<JsonResult> DonutMethod()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            Random rnd = new();

            List<Project> projects = (await _projectService.GetAllProjectsByCompany(companyId)).OrderBy(p => p.Id).ToList();

            DonutViewModel chartData = new();
            chartData.labels = projects.Select(p => p.Name).ToArray();

            List<SubData> dsArray = new();
            List<int> tickets = new();
            List<string> colors = new();

            foreach (Project prj in projects)
            {
                tickets.Add(prj.Tickets.Count());

                // This code will randomly select a color for each element of the data 
                Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                string colorHex = string.Format("#{0:X6}", randomColor.ToArgb() & 0X00FFFFFF);

                colors.Add(colorHex);
            }

            SubData temp = new()
            {
                data = tickets.ToArray(),
                backgroundColor = colors.ToArray()
            };
            dsArray.Add(temp);

            chartData.datasets = dsArray.ToArray();

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
