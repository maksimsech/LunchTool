﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Service.Implementation;

namespace LunchTool.Service.Implementation
{
    partial class AdministrationService
    {
        public class DishService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public DishService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MenuDTO, Menu>();
                }).CreateMapper();
            }

            public void Add(MenuDTO menuDTO)
            {

            }

            public void Change(MenuDTO menuDTO)
            {

            }

            public void Delete(MenuDTO menuDTO)
            {

            }
        }
    }
}
