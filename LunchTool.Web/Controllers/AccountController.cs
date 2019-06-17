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

namespace LunchTool.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private Service.Interfaces.IAuthenticationService authentificationService;
        private IMapper mapper;


        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            authentificationService = new Service.Implementation.AuthenticationService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginViewModel, UserDTO>();
                cfg.CreateMap<RegisterViewModel, UserDTO>();
            }).CreateMapper();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<LoginViewModel, UserDTO>(loginViewModel);
                if (authentificationService.CheckLogin(userDTO))
                {
                    userDTO.IsAdmin = authentificationService.IsAdmin(userDTO);
                    await Authentificate(userDTO.Email, userDTO.IsAdmin);

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<RegisterViewModel, UserDTO>(registerViewModel);
                if (authentificationService.IsRegistered(userDTO))
                {
                    ModelState.AddModelError("", "Пользователь уже существует");
                    return RedirectToAction("Register", "Account");
                }
                userDTO.IsAdmin = false;
                authentificationService.Register(userDTO);
                await Authentificate(userDTO.Email, userDTO.IsAdmin);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Account");
        }
 
        [NonAction]
        private async Task Authentificate(string email, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, isAdmin.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
    }
}
