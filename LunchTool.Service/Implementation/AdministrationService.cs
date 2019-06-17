using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Service.Implementation;
using LunchTool.Service.Interfaces.AdministrationServices;

namespace LunchTool.Service.Implementation
{
    public partial class AdministrationService : IAdministrationService
    {
        private readonly string connectionString;
        private IProviderService providerService;
        private IMenuService menuService;
        private IDishService dishService;

        public IProviderService Provider { get
            {
                if (providerService == null)
                    providerService = new ProviderService(connectionString);
                return providerService;
            } }

        public IMenuService Menu { get
            {
                if (menuService == null)
                    menuService = new MenuService(connectionString);
                return menuService;
            } }

        public IDishService Dish { get
            {
                if (dishService == null)
                    dishService = new DishService(connectionString);
                return dishService;
            } }

        public AdministrationService(string connectionString)
        {
            this.connectionString = connectionString;
        }

    }
}
