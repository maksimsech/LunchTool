using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Service.Interfaces.LoadDataServices;

namespace LunchTool.Service.Implementation
{
    public partial class LoadDataService
    {
        class UserService : IUserService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public UserService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDTO, User>();
                    cfg.CreateMap<User, UserDTO>();
                }).CreateMapper();
            }

            public IEnumerable<UserDTO> GetAll()
            {
                var users = db.Users.GetAll();
                var usersDTO = mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
                return usersDTO;
            }

            public IEnumerable<UserDTO> Get(Expression<Func<UserDTO, bool>> predicate)
            {
                var fun = mapper.MapExpression<Expression<Func<User, bool>>>(predicate).Compile();
                var users = db.Users.Find(fun);
                var usersDTO = mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
                return usersDTO;
            }
        }
    }
}
