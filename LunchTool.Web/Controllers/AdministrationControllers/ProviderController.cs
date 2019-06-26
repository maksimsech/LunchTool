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
            if (providerDTO == null)
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
            if (providerDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }
            administrationService.Provider.Deactivate(providerDTO);
            return RedirectToAction("Providers", "Administration");
        }

        [HttpPost]
        public IActionResult ActivateProvider(int id)
        {
            var providerDTO = dataService.Providers.Get(p => p.Id == id).FirstOrDefault();
            if (providerDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }
            administrationService.Provider.Activate(providerDTO);
            return RedirectToAction("Providers", "Administration");
        }

    }
}
