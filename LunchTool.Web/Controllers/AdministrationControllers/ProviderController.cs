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
            return View();
        }

        [HttpPost]
        public IActionResult GetProvidersPage(int pageNumber)
        {
            var providers = dataService.Providers.GetAll();

            var count = providers.Count();
            var currentPageItemsDTO = providers.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var currentPageItems = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(currentPageItemsDTO);
            var pageViewModel = new PageViewModel<IEnumerable<ProviderViewModel>>(currentPageItems, count, pageNumber, pageSize);
            return PartialView("~/Views/Shared/AdministrationPages/_ProvidersPage.cshtml", pageViewModel);
        }

        [HttpPost]
        public string AddProvider(ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerViewModel);
                administrationService.Provider.Add(providerDTO);
                return "Успешно добавлено";
            }
            //Temp solution
            return "Проверьте введеные данные";
        }

        [HttpGet]
        public IActionResult ChangeProvider(int id)
        {
            var providerDTO = dataService.Providers.Get(p => p.Id == id).FirstOrDefault();
            if (providerDTO == null)
            {
                //Temp solution
                return Content("Поставщик не найден");
            }

            var providerViewModel = mapper.Map<ProviderDTO, ProviderViewModel>(providerDTO);

            return PartialView("~/Views/Shared/AdministrationPages/_ChangeProvider.cshtml", providerViewModel);
        }

        [HttpPost]
        public string ChangeProvider(ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                var providerDTO = mapper.Map<ProviderViewModel, ProviderDTO>(providerViewModel);
                administrationService.Provider.Change(providerDTO);
                return "Успешно изменено";
            }
            return "Проверьте данные";
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
