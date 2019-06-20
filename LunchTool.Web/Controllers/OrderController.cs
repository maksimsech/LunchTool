using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using LunchTool.Service.Implementation;
using LunchTool.Service.Interfaces;
using LunchTool.Web.ViewModels;
using LunchTool.Service.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace LunchTool.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IConfiguration configuration;
        private LoadDataService dataService;
        private IOrderService orderService;
        private IMapper mapper;

        public OrderController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            dataService = new LoadDataService(connectionString);
            orderService = new OrderService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProviderDTO, ProviderViewModel>();
                cfg.CreateMap<MenuDTO, MenuViewModel>();
                cfg.CreateMap<DishDTO, DishViewModel>();
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.Identity.Name);
            var providersDTO = dataService.GetProviders(p => p.Active);
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            return View(providersViewModel);
        }

        [HttpPost]
        public IActionResult GetMenusById(int id)
        {
            var menusDTO = dataService.GetMenus(m => m.ProviderId == id);
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menusDTO);
            return PartialView("~/Views/Shared/_GetMenusById.cshtml", menusViewModel);
        }

        [HttpPost]
        public IActionResult GetDishesById(int id)
        {
            var dishesDTO = dataService.GetDishes(d => d.MenuId == id);
            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishesDTO);
            return PartialView("~/Views/Shared/_GetDishesById.cshtml", dishesViewModel);
        }
    }
}
