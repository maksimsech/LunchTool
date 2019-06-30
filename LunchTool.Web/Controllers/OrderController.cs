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
using LunchTool.Web.Models;
using System.Text;

namespace LunchTool.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IConfiguration configuration;
        private ILoadDataService dataService;
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
                cfg.CreateMap<UserDTO, UserForOrderViewModel>().ForMember(dest => dest.Name, src => src.MapFrom(user => $"{user.LastName} {user.FirstName} {user.Patronymic ?? ""}"));
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.Identity.Name);
            var providersDTO = dataService.Providers.Get(p => p.Active);
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            if(User.Identities.Any(u => u.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Administrator")))
            {
                var usersDTO = dataService.Users.GetAll();
                var users = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserForOrderViewModel>>(usersDTO);
                TempData["UserForAdmin"] = users;
            }
            return View(providersViewModel);
        }

        [HttpPost]
        public IActionResult GetMenusById(int id)
        {
            var time = DateTime.Now.TimeOfDay;
            var date = DateTime.Today;
            var menusDTO = dataService.Menus.Get(m => m.ProviderId == id && m.TimeLimit.TimeOfDay > time && m.Date.Date == date);
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menusDTO);
            return PartialView("~/Views/Shared/_GetMenusById.cshtml", menusViewModel);
        }

        [HttpPost]
        public IActionResult GetDishesById(int id)
        {
            var dishesDTO = dataService.Dishes.Get(d => d.MenuId == id);
            if (dishesDTO.Count() == 0)
                return Content("");
            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishesDTO);
            return PartialView("~/Views/Shared/_GetDishesById.cshtml", dishesViewModel);
        }

        [HttpPost]
        public IActionResult GetProvidersByDate(DateTime date)
        {
            var time = DateTime.Now.TimeOfDay;
            IEnumerable<int> findedIds;
            if (date.Date < DateTime.Today)
                return Content("Неверная дата");
            if (date.Date == DateTime.Today)
            {
                findedIds = dataService.Menus.Get(m => m.Date.Date == date.Date && m.TimeLimit.TimeOfDay > time && m.IsActive)
                                                .GroupBy(m => m.ProviderId)
                                                .Select(m => m.First())
                                                .Select(m => m.ProviderId);
            }
            else
            {
                findedIds = dataService.Menus.Get(m => m.Date.Date == date.Date && m.IsActive)
                                                .GroupBy(m => m.ProviderId)
                                                .Select(m => m.First())
                                                .Select(m => m.ProviderId);
            }
            var providersDTO = dataService.Providers.Get(p => findedIds.Contains(p.Id));
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            return PartialView("~/Views/Shared/_GetProvidersByDate.cshtml", providersViewModel);
        }

        [HttpPost]
        public IActionResult GetDishesByProviderId(int id, DateTime date)
        {
            var menuDTO = dataService.Menus.Get(m => m.ProviderId == id && m.Date.Date == date && m.IsActive).FirstOrDefault();
            if ( menuDTO == null)
            {
                return Content("Меню не найдено");
            }
            var dishesDTO = dataService.Dishes.Get(d => d.MenuId == menuDTO.Id);
            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishesDTO);
            return PartialView("~/Views/Shared/_GetDishesById.cshtml", dishesViewModel);
        }

        [HttpPost]
        public string MakeOrder(IEnumerable<DishForOrderModel> dishesForOrderModel)
        {
            if (ModelState.IsValid)
            {
                if(dishesForOrderModel.Count() == 0 || dishesForOrderModel.All(d => d.Count == 0))
                {
                    return "Нужно быбрать минимум одно блюдо";
                }
                int userId = -1;
                if(User.Identities.Any(u => u.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Administrator")))
                {
                    if(Request.Form.TryGetValue("userIdForAdmin", out var userIdForAdmin))
                    {
                        userId = int.Parse(userIdForAdmin);
                    }
                    else
                    {
                        return "Пользователь не найден";
                    }
                }
                else userId = int.Parse(User.Identity.Name);
                var orderDTO = new OrderDTO
                {
                    CreateDate = DateTime.Now,
                    UserId = userId
                };
                var id = orderService.MakeOrder(orderDTO);

                foreach(var item in dishesForOrderModel)
                {
                    if (item.Count != 0)
                    {
                        var orderDishDTO = new OrderDishDTO
                        {
                            OrderId = id,
                            DishId = item.Id,
                            Count = item.Count
                        };
                        orderService.AddOrderDish(orderDishDTO);
                    }
                }
                return "Заказ выполнен.";
            }
            return "Ошибка при проверке данных";
        }
    }
}
