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
            if (userDTO == null)
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
