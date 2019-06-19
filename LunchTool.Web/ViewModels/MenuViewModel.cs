using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeLimit { get; set; }

        public int ProviderId { get; set; }
    }
}
