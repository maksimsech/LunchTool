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
using System.Text;

namespace LunchTool.Web.Controllers
{
    [Authorize(Policy = "AdministrationOnly")]
    public class AdministrationController : Controller
    {
        private readonly IConfiguration configuration;
        private LoadDataService dataService;
        private AdministrationService administrationService;
        private IMapper mapper;

        public AdministrationController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            dataService = new LoadDataService(connectionString);
            administrationService = new AdministrationService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProviderViewModel, ProviderDTO>();
                cfg.CreateMap<ProviderDTO, ProviderViewModel>();

                cfg.CreateMap<MenuViewModel, MenuDTO>();
                cfg.CreateMap<MenuDTO, MenuViewModel>();

                cfg.CreateMap<DishDTO, DishViewModel>();
                cfg.CreateMap<DishViewModel, DishDTO>();

                cfg.CreateMap<UserViewModel, UserDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Providers()
        {
            var providersDTO = dataService.Providers.GetAll();
            var providers = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            return View(providers);
        }

        [HttpPost]
        public IActionResult AddProvider(ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerViewModel);
                administrationService.Provider.Add(providerDTO);
                return RedirectToAction("Providers", "Administration");
            }
            //Temp solution
            return Content("Проверьте введеные данные");
        }

        [HttpGet("[controller]/Provider/{id}/Change")]
        public IActionResult ChangeProvider(int id)
        {
            var providerDTO = dataService.Providers.Get(p => p.Id == id).FirstOrDefault();
            if(providerDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }

            var providerViewModel = mapper.Map<ProviderDTO, ProviderViewModel>(providerDTO);

            return View(providerViewModel);
        }

        [HttpPost("[controller]/Provider/{id}/Change")]
        public IActionResult ChangeProvider(ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerViewModel);
                administrationService.Provider.Change(providerDTO);
                return RedirectToAction("Providers", "Administration");
            }
            return Content("Проверьте данные");
        }

        [HttpPost]
        public IActionResult DeactivateProvider(int id)
        {
            var providerDTO = dataService.Providers.Get(p => p.Id == id).FirstOrDefault();
            if(providerDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }
            administrationService.Provider.Deactivate(providerDTO);
            return RedirectToAction("Providers", "Administration");
        }

        public IActionResult Menus(int? id)
        {
            IEnumerable<MenuDTO> menus;
            IEnumerable<ProviderDTO> providers;
            if (id == null)
            {
                menus = dataService.Menus.GetAll();
                providers = dataService.Providers.GetAll();
            }
            else
            {
                menus = dataService.Menus.Get(m => m.ProviderId == id);
                providers = dataService.Providers.Get(p => p.Id == id);
            }
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menus);
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providers);
            return View(new Tuple<IEnumerable<MenuViewModel>, IEnumerable<ProviderViewModel>>(menusViewModel, providersViewModel));
        }

        [HttpPost]
        public IActionResult AddMenu(MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                var menuDTO = mapper.Map<MenuViewModel, MenuDTO>(menuViewModel);
                administrationService.Menu.Add(menuDTO);
                return RedirectToAction("Menus", "Administration");
            }
            //Temp solution
            return Content("Проверьте введенные данные");
        }

        [HttpGet("[controller]/Menu/{id}/Change")]
        public IActionResult ChangeMenu(int id)
        {
            var menuDTO = dataService.Menus.Get(p => p.Id == id).FirstOrDefault();
            if (menuDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }

            var menuViewModel = mapper.Map<MenuDTO, MenuViewModel>(menuDTO);

            return View(menuViewModel);
        }

        [HttpPost("[controller]/Menu/{id}/Change")]
        public IActionResult ChangeMenu(MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                var menuDTO = mapper.Map<MenuViewModel, MenuDTO>(menuViewModel);
                administrationService.Menu.Change(menuDTO);
                return RedirectToAction("Menus", "Administration");
            }
            return Content("Проверьте введенные данные");
        }

        [HttpPost]
        public IActionResult DeleteMenu(int id)
        {
            var menuDTO = dataService.Menus.Get(m => m.Id == id).FirstOrDefault();
            if(menuDTO == null)
            {
                //Temp solution
                return Content("Меню не найдено");
            }
            administrationService.Menu.Delete(menuDTO);
            return View("Menus", "Administration");
        }

        public IActionResult Dishes(int? id)
        {
            IEnumerable<DishDTO> dishes;
            IEnumerable<ProviderDTO> providers = null;

            if (id == null)
            {
                dishes = dataService.Dishes.GetAll();
                providers = dataService.Providers.GetAll();
            }
            else
                dishes = dataService.Dishes.Get(d => d.MenuId == id);

            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishes);
            IEnumerable<ProviderViewModel> providersViewModel = null;
            if (providers != null)
            {
                providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providers);
            }
            else
            {
                TempData["MenuId"] = id;
            }
            return View(new Tuple<IEnumerable<DishViewModel>, IEnumerable<ProviderViewModel>>(dishesViewModel, providersViewModel));
        }

        [HttpPost]
        public IActionResult GetMenusById(int id)
        {
            var menusDTO = dataService.Menus.Get(m => m.ProviderId == id);
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menusDTO);
            return PartialView("~/Views/Shared/_GetMenusById.cshtml", menusViewModel);
        }

        [HttpPost]
        public IActionResult AddDish(DishViewModel dishViewModel, int MenuId)
        {
            if (ModelState.IsValid)
            {
                dishViewModel.MenuId = MenuId;
                var dishDTO = mapper.Map<DishViewModel, DishDTO>(dishViewModel);
                administrationService.Dish.Add(dishDTO);
                return RedirectToAction("Dishes", "Administration");
            }
            return Content("Проверьте данные");
        }


        [HttpGet("[controller]/Dish/{id}/Change")]
        public IActionResult ChangeDish(int id)
        {
            var dishDTO = dataService.Dishes.Get(d => d.Id == id).FirstOrDefault();
            if(dishDTO == null)
            {
                //Temp solution
                return Content("Блюдо не найдено");
            }

            var dishViewModel = mapper.Map<DishDTO, DishViewModel>(dishDTO);
            return View(dishViewModel);
        }

        [HttpPost("[controller]/Dish/{id}/Change")]
        public IActionResult ChangeDish(DishViewModel dishViewModel)
        {
            if (ModelState.IsValid)
            {
                var dishDTO = mapper.Map<DishViewModel, DishDTO>(dishViewModel);
                administrationService.Dish.Change(dishDTO);
                return RedirectToAction("Dishes", "Administration");
            }
            return Content("Проверьте данные");
        }

        [HttpPost]
        public IActionResult DeleteDish(DishViewModel dishViewModel)
        {
            var dishDTO = mapper.Map<DishViewModel, DishDTO>(dishViewModel);
            administrationService.Dish.Delete(dishDTO);
            return RedirectToAction("Dishes", "Administration");
        }

        public IActionResult Users()
        {
            var usersDTO = dataService.Users.GetAll();
            var usersViewModel = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(usersDTO);
            return View(usersViewModel);
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                if (!administrationService.Users.IsRegistered(userDTO))
                {
                    administrationService.Users.Add(userDTO);
                    return Content("Зарегестрирован");
                }
                return Content("Уже зарегестрирован");
            }
            return Content("Проверьте данные");
        }

        [HttpGet]
        public IActionResult ChangeUser(int id)
        {
            var userDTO = dataService.Users.Get(u => u.Id == id).FirstOrDefault();
            if(userDTO == null)
            {
                return Content("Пользователь не найден");
            }
            var userViewModel = mapper.Map<UserDTO, UserViewModel>(userDTO);
            return View(userViewModel);
        }


        [HttpPost]
        public IActionResult ChangeUser(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                var find = dataService.Users.Get(u => u.Id == userDTO.Id).FirstOrDefault();
                userDTO.Password = find.Password; 
                administrationService.Users.Change(userDTO);
                return RedirectToAction("Users", "Administration");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var sb = new StringBuilder();
            foreach (var error in errors)
                sb.AppendLine(error.ErrorMessage);
            return Content(sb.ToString());
        }
    }
}
