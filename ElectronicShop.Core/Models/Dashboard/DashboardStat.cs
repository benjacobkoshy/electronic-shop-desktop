using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Core.Models.Dashboard
{
    public class DashboardStat
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string AccentBrushKey { get; set; } = string.Empty; // "PrimaryBrush", "AccentBrush", "DangerBrush" etc.
    }
}