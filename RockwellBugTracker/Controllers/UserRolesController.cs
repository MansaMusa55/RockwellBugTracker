using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RockwellBugTracker.Data;
using RockwellBugTracker.Extensions;
using RockwellBugTracker.Models;
using RockwellBugTracker.Models.Enums;
using RockwellBugTracker.Models.ViewModel;
using RockwellBugTracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Controllers
{
    //[Authorize(Roles="Admin")]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRoleService _rolesService;
        private readonly IBTCompanyInfoService _infoService;

        public UserRolesController(ApplicationDbContext context,
                                   IBTRoleService rolesService, IBTCompanyInfoService infoService)
        {
            _context = context;
            _rolesService = rolesService;
            _infoService = infoService;
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new();
            int companyId = User.Identity.GetCompanyId().Value;
            //TODO: Company users
            List<BTUser> users = await _infoService.GetAllMembersAsync(companyId);

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new();
                vm.BTUser = user;
                var selected = await _rolesService.ListUserRolesAsync(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);
                model.Add(vm);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member, string selectedRole, string btUserId)
        {
            BTUser user = _context.Users.Find(btUserId);

            IEnumerable<string> roles = await _rolesService.ListUserRolesAsync(user);
            await _rolesService.RemoveUserFromRolesAsync(user, roles);

            if (Enum.TryParse(selectedRole, out Roles roleValue))
            {
                await _rolesService.AddUserToRoleAsync(user, selectedRole);
                return RedirectToAction("ManageUserRoles");
            }

            return RedirectToAction("ManageUserRoles");

        }

    }
}
