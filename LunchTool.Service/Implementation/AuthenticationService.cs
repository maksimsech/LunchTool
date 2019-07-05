using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
                cfg.CreateMap<User, AuthUserDTO>();
            }).CreateMapper();
        }

        public bool IsRegistered(UserDTO userDTO)
        {
            
            var user = MapToUser(userDTO);
            var find = db.Users.Find(u => u.Email == user.Email);
            return find != null;
        }

        public void Register(UserDTO userDTO)
        {
            if (IsRegistered(userDTO))
                throw new ValidationException("", "Пользователь уже зарегестрирован");
            var user = MapToUser(userDTO);
            db.Users.Add(user);
            db.Save();
        }

        public bool CheckLogin(UserDTO userDTO)
        {
            var user = MapToUser(userDTO);
            var find = db.Users.Find(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            return find != null;
        }

        public bool IsAdmin(UserDTO userDTO)
        {
            var user = MapToUser(userDTO);
            var find = db.Users.Find(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (find == null)
            {
                throw new ValidationException("","Пользователь не найден");
            }
            return find.IsAdmin;
        }

        public AuthUserDTO GetAuthUserDTO(UserDTO userDTO)
        {
            var user = MapToUser(userDTO);
            var find = db.Users.Find(u => u.Email == user.Email).FirstOrDefault();
            if (find == null)
            {
                throw new ValidationException("", "Пользователь не найден");
            }
            var authUser = mapper.Map<User, AuthUserDTO>(find);
            return authUser;
        }

        public void ChangeInfo(UserDTO userDTO)
        {
            var user = mapper.Map<UserDTO, User>(userDTO);
            db.Users.Update(user);
            db.Save();
        }

        public void ChangePassword(int id, string newPassword)
        {
            var user = db.Users.Get(id);
            user.Password = newPassword;
            db.Users.Update(user);
            db.Save();
        }

        public bool CheckPassword(int id, string password)
        {
            var user = db.Users.Get(id);
            return password == user.Password;
        }

        private User MapToUser(UserDTO userDTO) => mapper.Map<UserDTO, User>(userDTO);
    }
}
