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
    public partial class AdministrationController
    {
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
            if (dishDTO == null)
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
    }
}
