﻿using System;
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
                menuDTO.IsActive = true;
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

        [HttpGet]
        public IActionResult CopyMenu(int MenuId)
        {
            var menuDTO = dataService.Menus.Get(m => m.Id == MenuId).FirstOrDefault();
            if (menuDTO == null)
            {
                return Content("Меню не найдено");
            }
            var menuViewModel = mapper.Map<MenuDTO, MenuViewModel>(menuDTO);
            var dishesDTO = dataService.Dishes.Get(d => d.MenuId == MenuId);
            var dishesViewModel = mapper.Map<IEnumerable<DishDTO>, IEnumerable<DishViewModel>>(dishesDTO);
            return View(new Tuple<MenuViewModel, IEnumerable<DishViewModel>>(menuViewModel, dishesViewModel));
        }

        [HttpPost]
        public IActionResult CopyMenu(MenuViewModel menuViewModel, IEnumerable<CopyDishViewModel> copyDishViewModels)
        {
            if (ModelState.IsValid)
            {
                var menuDTO = mapper.Map<MenuViewModel, MenuDTO>(menuViewModel);
                menuDTO.IsActive = true;
                var menuId = administrationService.Menu.Add(menuDTO);
                var dishesDTO = new List<DishDTO>();
                foreach (var dish in copyDishViewModels)
                {
                    if (dish.IsSelected)
                    {
                        var dishDTO = new DishDTO
                        {
                            Name = dish.Name,
                            Price = dish.Price,
                            Weight = dish.Weight,
                            MenuId = menuId
                        };
                        administrationService.Dish.Add(dishDTO);
                    }
                }
                return RedirectToAction("Menus", "Administration");
            }
            return Content("Проверьте введенные данные");
        }

        [HttpPost]
        public IActionResult GetMenusById(int idMenu)
        {
            var menusDTO = dataService.Menus.Get(m => m.ProviderId == idMenu);
            var menusViewModel = mapper.Map<IEnumerable<MenuDTO>, IEnumerable<MenuViewModel>>(menusDTO);
            return PartialView("~/Views/Shared/_GetMenusById.cshtml", menusViewModel);
        }

        [HttpPost]
        public IActionResult DeactivateMenu(int id)
        {
            var menuDTO = dataService.Menus.Get(m => m.Id == id).FirstOrDefault();
            if (menuDTO == null)
            {
                return Content("Меню не найден");
            }
            administrationService.Menu.Deactivate(menuDTO);
            return RedirectToAction("Menu", "Administration");
        }

        [HttpPost]
        public IActionResult ActivateMenu(int id)
        {
            var menuDTO = dataService.Menus.Get(m => m.Id == id).FirstOrDefault();
            if (menuDTO == null)
            {
                return Content("Меню не найден");
            }
            administrationService.Menu.Activate(menuDTO);
            return RedirectToAction("Menu", "Administration");
        }

        [HttpPost]
        public IActionResult DeleteMenu(int id)
        {
            var menuDTO = dataService.Menus.Get(m => m.Id == id).FirstOrDefault();
            if (menuDTO == null)
            {
                //Temp solution
                return Content("Меню не найдено");
            }
            administrationService.Menu.Delete(menuDTO);
            return View("Menus", "Administration");
        }
    }
}