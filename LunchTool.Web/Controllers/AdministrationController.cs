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
        public IActionResult DeleteProvider(ProviderViewModel providerModelView)
        {
            var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerModelView);
            administrationService.Provider.Deactivate(providerDTO);
            return View("Providers", "Administration");
        }

        public IActionResult Menus(int? id)
        {
            IEnumerable<MenuDTO> menus;
            if (id == null)
                menus = dataService.GetAllMenus();
            else
                menus = dataService.GetMenus(m => m.ProviderId == id);
            return View(menus);
        }

        public IActionResult Dishes(int? id)
        {
            IEnumerable<DishDTO> dishes;
            if (id == null)
                dishes = dataService.GetAllDishes();
            else
                dishes = dataService.GetDishes(d => d.MenuId == id);
            return View(dishes);
        }
    }
}
