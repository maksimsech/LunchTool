﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
    }
}
