using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces.AdministrationServices;
using LunchTool.Service.Implementation;
using System.Linq;

namespace LunchTool.Service.Implementation
{
    partial class AdministrationService
    {
        public class UserService : IUserService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public UserService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDTO, User>();
                }).CreateMapper();
            }

            public void Add(UserDTO userDTO)
            {
                var user = Map(userDTO);
                db.Users.Add(user);
                db.Save();
            }

            public void Change(UserDTO userDTO)
            {
                var user = Map(userDTO);
                db.Users.Update(user);
                db.Save();
            }

            public bool IsRegistered(UserDTO userDTO)
            {
                var find = db.Users.Find(u => u.Email == userDTO.Email).FirstOrDefault();
                return find != null;
            }

            private User Map(UserDTO userDTO) => mapper.Map<UserDTO, User>(userDTO);
        }
    }
}
