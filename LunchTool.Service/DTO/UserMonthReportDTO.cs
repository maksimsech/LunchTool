using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class UserMonthReportDTO
    {
        public int Day { get; set; }
        public int OrderCount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
}
