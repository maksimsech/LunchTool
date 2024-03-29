﻿using System;
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
        bool IsAdmin(UserDTO userDTO);
        AuthUserDTO GetAuthUserDTO(UserDTO userDTO);
        void ChangeInfo(UserDTO userDTO);
        bool CheckPassword(int id, string password);
        void ChangePassword(int id, string newPassword);
    }
}
