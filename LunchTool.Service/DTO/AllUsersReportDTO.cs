using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class AllUsersReportDTO
    {
        public string UserName { get; set; }
        public int OrderCount { get; set; }
        public decimal Price { get; set; }
    }
}
