using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class UserProvidersReportDTO
    {
        public string ProviderName { get; set; }
        public string UserName { get; set; }
        public int OrderCount { get; set; }
        public decimal Price { get; set; }
    }
}
