using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class AuthUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
