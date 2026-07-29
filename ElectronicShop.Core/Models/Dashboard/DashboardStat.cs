using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Core.Models.Dashboard
{
    public class DashboardStat
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string AccentBrushKey { get; set; } // "PrimaryBrush", "AccentBrush", "DangerBrush" etc.
    }
}