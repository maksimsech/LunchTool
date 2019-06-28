using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class UserPageReportDTO
    {
        public DateTime OrderDate { get; set; }
        public string ProviderName { get; set; }
        public string Dishes { get; set; }
        public decimal Price { get; set; }
    }
}
