using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    interface IUserService
    {
        void Add(UserDTO userDTO);
        void Change(UserDTO userDTO);
    }
}
