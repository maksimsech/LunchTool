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
using System.Security.Cryptography;

namespace LunchTool.Web.Controllers
{
    public partial class AdministrationController
    {
        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUsersPage(int pageNumber)
        {
            var users = dataService.Users.GetAll();
            var count = users.Count();
            var currentPageItemsDTO = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var currentPageItems = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(currentPageItemsDTO);
            var pageViewModel = new PageViewModel<IEnumerable<UserViewModel>>(currentPageItems, count, pageNumber, pageSize);
            return PartialView("~/Views/Shared/AdministrationPages/_UsersPage.cshtml", pageViewModel);
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                if (!administrationService.User.IsRegistered(userDTO))
                {
                    administrationService.User.Add(userDTO);
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
            if (userDTO == null)
            {
                return Content("Пользователь не найден");
            }
            var userViewModel = mapper.Map<UserDTO, UserViewModel>(userDTO);
            return PartialView("~/Views/Shared/AdministrationPages/_ChangeUser.cshtml", userViewModel);
        }


        [HttpPost]
        public IActionResult ChangeUser(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                var find = dataService.Users.Get(u => u.Id == userDTO.Id).FirstOrDefault();
                userDTO.Password = find.Password;
                administrationService.User.Change(userDTO);
                return Content("Данные изменены");
            }
            return Content("Проверьте данные");
        }
    }
}
