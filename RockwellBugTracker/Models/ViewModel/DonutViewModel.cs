using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Models.ViewModel
{
    public class DonutViewModel
    {
        public string[] labels { get; set; }
        public SubData[] datasets { get; set; }
    }

    public class SubData
    {
        public int[] data { get; set; }
        public string[] backgroundColor { get; set; }
    }
}
