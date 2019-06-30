using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class UserPageReportViewModel
    {
        public DateTime OrderDate { get; set; }
        public string ProviderName { get; set; }
        public string Dishes { get; set; }
        public decimal Price { get; set; }
    }
}
