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
        public class DishService: IDishService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public DishService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DishDTO, Dish>();
                    cfg.CreateMap<Dish, DishDTO>();
                }).CreateMapper();
            }

            public IEnumerable<DishDTO> GetAll()
            {
                var dishes = db.Dishes.GetAll();
                var dishesDTO = mapper.Map<IEnumerable<Dish>, IEnumerable<DishDTO>>(dishes);
                return dishesDTO;
            }

            public IEnumerable<DishDTO> Get(Expression<Func<DishDTO, bool>> predicate)
            {
                var fun = mapper.MapExpression<Expression<Func<Dish, bool>>>(predicate).Compile();
                var dishes = db.Dishes.Find(fun);
                var dishesDTO = mapper.Map<IEnumerable<Dish>, IEnumerable<DishDTO>>(dishes);
                return dishesDTO;
            }
        }
    }
}
