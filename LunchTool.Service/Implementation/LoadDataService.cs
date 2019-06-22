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
    public partial class LoadDataService : ILoadDataService
    {
        private string connectionString;
        private IProviderService providerService;
        private IMenuService menuService;
        private IDishService dishService;

        public LoadDataService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IProviderService Providers { get
            {
                if (providerService == null)
                    providerService = new ProviderService(connectionString);
                return providerService;
            } }

        public IMenuService Menus { get
            {
                if (menuService == null)
                    menuService = new MenuService(connectionString);
                return menuService;
            } }

        public IDishService Dishes { get
            {
                if (dishService == null)
                    dishService = new DishService(connectionString);
                return dishService;
            } }
    }
}
