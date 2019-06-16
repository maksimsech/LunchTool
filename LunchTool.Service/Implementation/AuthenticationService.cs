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
    public class AuthenticationService : IAuthenticationService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public AuthenticationService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
            }).CreateMapper();
        }

        public bool IsRegistered(UserDTO userDTO)
        {
            
            var user = mapper.Map<UserDTO, User>(userDTO);
            var find = db.Users.Find(u => u.Username == user.Username && u.Email == user.Email);
            return find == null;
        }

        public void Register(UserDTO userDTO)
        {
            if (IsRegistered(userDTO))
                throw new ValidationException("AuthenticationService", "Пользователь уже зарегестрирован");
            var user = mapper.Map<UserDTO, User>(userDTO);
            db.Users.Add(user);
            db.Save();
        }

        public bool CheckLogin(UserDTO userDTO)
        {
            var user = mapper.Map<UserDTO, User>(userDTO);
            var find = db.Users.Find(u => u.Email == user.Password && u.Password == user.Password);
            return find != null;
        }
    }
}
