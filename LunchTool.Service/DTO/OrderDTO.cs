using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }
    }
}
