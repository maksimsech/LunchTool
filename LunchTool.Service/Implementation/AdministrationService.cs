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

namespace LunchTool.Service.Implementation
{
    public partial class AdministrationService// : IAdministrationService
    {
        private readonly string connectionString;
        private ProviderService providerService;
        private MenuService menuService;
        private DishService dishService;

        public ProviderService Provider { get
            {
                if (providerService == null)
                    providerService = new ProviderService(connectionString);
                return providerService;
            } }

        public MenuService Menu { get
            {
                if (menuService == null)
                    menuService = new MenuService(connectionString);
                return menuService;
            } }

        public DishService Dish { get
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
