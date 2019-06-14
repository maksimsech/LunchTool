using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    class MenuDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }

        public int ProviderId { get; set; }
    }
}
