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

namespace LunchTool.Service.Implementation
{
    public class LoadDataService : ILoadDataService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public LoadDataService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProviderDTO, Provider>();
                cfg.CreateMap<Provider, ProviderDTO>();
                cfg.CreateMap<MenuDTO, Menu>();
                cfg.CreateMap<Menu, MenuDTO>();
                cfg.CreateMap<DishDTO, Dish>();
                cfg.CreateMap<Dish, DishDTO>();
                cfg.CreateMap<Func<ProviderDTO, bool>, Func<Provider, bool>>();
                cfg.CreateMap<Expression<Func<ProviderDTO, bool>>, Expression<Func<Provider, bool>>>();
            }).CreateMapper();
        }

        public IEnumerable<ProviderDTO> GetAllProviders()
        {
            var providers = db.Providers.GetAll();
            var providersDTO = mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderDTO>>(providers);
            return providersDTO;
        }

        public IEnumerable<ProviderDTO> GetProviders(Expression<Func<ProviderDTO, bool>> predicate)
        {
            var fun = mapper.MapExpression<Expression<Func<Provider, bool>>>(predicate).Compile();
            var providers = db.Providers.Find(fun);
            var providersDTO = mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderDTO>>(providers);
            return providersDTO;
        }

        public IEnumerable<MenuDTO> GetAllMenus()
        {
            var menus = db.Menus.GetAll();
            var menusDTO = mapper.Map<IEnumerable<Menu>, IEnumerable<MenuDTO>>(menus);
            return menusDTO;
        }

        public IEnumerable<MenuDTO> GetMenus(Expression<Func<MenuDTO, bool>> predicate)
        {
            var fun = mapper.MapExpression<Expression<Func<Menu, bool>>>(predicate).Compile();
            var menus = db.Menus.Find(fun);
            var menusDTO = mapper.Map<IEnumerable<Menu>, IEnumerable<MenuDTO>>(menus);
            return menusDTO;
        }

        public IEnumerable<DishDTO> GetAllDishes()
        {
            var dishes = db.Dishes.GetAll();
            var dishesDTO = mapper.Map<IEnumerable<Dish>, IEnumerable<DishDTO>>(dishes);
            return dishesDTO;
        }

        public IEnumerable<DishDTO> GetDishes(Expression<Func<DishDTO, bool>> predicate)
        {
            var fun = mapper.MapExpression<Expression<Func<Dish, bool>>>(predicate).Compile();
            var dishes = db.Dishes.Find(fun);
            var dishesDTO = mapper.Map<IEnumerable<Dish>, IEnumerable<DishDTO>>(dishes);
            return dishesDTO;
        }
    }
}
