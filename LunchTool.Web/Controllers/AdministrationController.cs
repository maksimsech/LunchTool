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

namespace LunchTool.Web.Controllers
{
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
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Providers()
        {
            var providersDTO = dataService.GetAllProviders();
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
            var providerDTO = dataService.GetProviders(p => p.Id == id).FirstOrDefault();
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
            var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerViewModel);
            administrationService.Provider.Change(providerDTO);
            return RedirectToAction("Providers", "Administration");
        }

        [HttpPost]
        public IActionResult DeactivateProvider(ProviderViewModel providerModelView)
        {
            var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerModelView);
            administrationService.Provider.Deactivate(providerDTO);
            return View("Providers", "Administration");
        }

        public IActionResult Menus(int? id)
        {
            IEnumerable<MenuDTO> menus;
            IEnumerable<ProviderDTO> providers;
            if (id == null)
            {
                menus = dataService.GetAllMenus();
                providers = dataService.GetAllProviders();
            }
            else
            {
                menus = dataService.GetMenus(m => m.ProviderId == id);
                providers = dataService.GetProviders(p => p.Id == id);
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
            var menuDTO = dataService.GetMenus(p => p.Id == id).FirstOrDefault();
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
            var menuDTO = mapper.Map<MenuViewModel, MenuDTO>(menuViewModel);
            administrationService.Menu.Change(menuDTO);
            return RedirectToAction("Menus", "Administration");
        }

        [HttpPost]
        public IActionResult DeleteMenu(MenuViewModel menuViewModel)
        {
            var menuDTO = mapper.Map<MenuViewModel, MenuDTO>(menuViewModel);
            administrationService.Menu.Delete(menuDTO);
            return RedirectToAction("Menus","Administration");
        }

        public IActionResult Dishes(int? id)
        {
            IEnumerable<DishDTO> dishes;
            IEnumerable<ProviderDTO> providers = null;

            if (id == null)
            {
                dishes = dataService.GetAllDishes();
                providers = dataService.GetAllProviders();
            }
            else
                dishes = dataService.GetDishes(d => d.MenuId == id);

            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishes);
            IEnumerable<ProviderViewModel> providersViewModel = null;
            if (providers != null)
            {
                providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providers);
            }
            return View(new Tuple<IEnumerable<DishViewModel>, IEnumerable<ProviderViewModel>>(dishesViewModel, providersViewModel));
        }

        [HttpPost]
        public IActionResult GetMenusById(int id)
        {
            var menusDTO = dataService.GetMenus(m => m.ProviderId == id);
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menusDTO);
            return PartialView("~/Views/Shared/_GetMenusById.cshtml", menusViewModel);
        }
    }
}
