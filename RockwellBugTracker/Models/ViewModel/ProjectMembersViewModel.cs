using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models.ViewModel
{
    public class ProjectMembersViewModel
    {
        public Project Project { get; set; } = new();
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}
