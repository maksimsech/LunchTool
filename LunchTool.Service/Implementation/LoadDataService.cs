﻿using System;
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
    public class LoadDataService// : ILoadDataService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public LoadDataService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProviderDTO, Provider>();
                cfg.CreateMap<MenuDTO, Menu>();
                cfg.CreateMap<DishDTO, Dish>();
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

        public IEnumerable<ProviderDTO> GetProviders(Func<ProviderDTO, bool> predicate)
        {
            Expression<Func<ProviderDTO, bool>> expr = e => predicate(e);
            var dto = new ProviderDTO
            {
                Name = "a",
                Email = "a"
            };
            var entity = mapper.Map<ProviderDTO, Provider>(dto);
            var a = mapper.Map<Func<ProviderDTO, bool>, Func<Provider, bool>>(predicate);
            var providersExpr = mapper.MapExpressionAsInclude<Expression<Func<ProviderDTO, bool>>, Expression<Func<Provider, bool>>>(expr);
            var fun = providersExpr.Compile();
            var providers = db.Providers.Find(fun);
            var providersDTO = mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderDTO>>(providers);
            return providersDTO;
        }
    }
}