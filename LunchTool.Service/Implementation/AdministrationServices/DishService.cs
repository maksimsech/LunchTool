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

namespace LunchTool.Service.Implementation
{
    partial class AdministrationService
    {
        public class DishService : IDishService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public DishService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DishDTO, Dish>();
                }).CreateMapper();
            }

            public void Add(DishDTO dishDTO)
            {

            }

            public void Change(DishDTO dishDTO)
            {

            }

            public void Delete(DishDTO dishDTO)
            {

            }
        }
    }
}
