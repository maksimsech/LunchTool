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
        public class MenuService : IMenuService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public MenuService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MenuDTO, Menu>();
                }).CreateMapper();
            }

            public int Add(MenuDTO menuDTO)
            {
                var menu = Map(menuDTO);
                db.Menus.Add(menu);
                db.Save();
                return menu.Id;
            }

            public void Change(MenuDTO menuDTO)
            {
                var menu = Map(menuDTO);
                db.Menus.Update(menu);
                db.Save();
            }

            public void Delete(MenuDTO menuDTO)
            {
                var menu = Map(menuDTO);
                db.Menus.Delete(menu);
                db.Save();
            }

            private Menu Map(MenuDTO menuDTO) => mapper.Map<MenuDTO, Menu>(menuDTO);
        }
    }
}
