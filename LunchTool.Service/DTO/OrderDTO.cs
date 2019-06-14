using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    class OrderDTO
    {
        public int Id { get; set; }
        public DateTime TimeLimit { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
    }
}
