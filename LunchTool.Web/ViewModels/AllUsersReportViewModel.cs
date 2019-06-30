using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class AllUsersReportViewModel
    {
        public string UserName { get; set; }
        public int OrderCount { get; set; }
        public decimal Price { get; set; }
    }
}
