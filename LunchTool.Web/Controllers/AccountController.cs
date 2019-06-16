using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private IAuthenticationService authentificationService;
        private IMapper mapper;


        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            authentificationService = new AuthenticationService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginViewModel, UserDTO>();
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
                    await Authentificate(loginViewModel.Email);

                    return RedirectToAction("Index", "Index");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [NonAction]
        private async Task Authentificate(string email)
        {

        }
        
    }
}
