using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class UserReportViewModel
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string ProviderName { get; set; }
        public IEnumerable<DishViewModel> Dishes { get; set; }
        public decimal Price { get; set; }
    }
}
