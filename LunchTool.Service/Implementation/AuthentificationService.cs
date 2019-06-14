using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Service.Infrastracture;
using AutoMapper;

namespace LunchTool.Service.Implementation
{
    public class AuthentificationService : IAuthentificationService
    {
        private IUnitOfWork db;

        public AuthentificationService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
        }

        public bool IsRegistered(UserDTO userDTO)
        {
            var user = Mapper.Map<UserDTO, User>(userDTO);
            var find = db.Users.Find(u => u.Username == user.Username && u.Email == user.Email);
            return find == null;
        }

        public void Register(UserDTO userDTO)
        {
            if (IsRegistered(userDTO))
                throw new ValidationException("AuthentificationService", "Пользователь уже зарегестрирован");
            var user = Mapper.Map<UserDTO, User>(userDTO);
            db.Users.Add(user);
            db.Save();
        }

        public bool CheckLogin(UserDTO userDTO)
        {
            var user = Mapper.Map<UserDTO, User>(userDTO);
            var find = db.Users.Find(u => u.Username == user.Username && u.Password == user.Password);
            return find != null;
        }
    }
}
