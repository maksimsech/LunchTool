using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class UserMonthReportViewModel
    {
        public int Day { get; set; }
        public int OrderCount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
}
