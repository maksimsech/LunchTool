using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsRegistered(UserDTO userDTO);
        void Register(UserDTO userDTO);
        bool CheckLogin(UserDTO userDTO);
    }
}
