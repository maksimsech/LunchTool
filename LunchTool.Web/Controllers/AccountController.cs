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

namespace LunchTool.Web.Controllers
{
    [AllowAnonymous]
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

        public IActionResult Index()
        {
            return Content("Кабинет");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identities.Any(u => u.IsAuthenticated))
                return RedirectToAction("Index", "Home");
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
                    var authUserDTO = authentificationService.GetAuthUserDTO(userDTO);
                    await Authentificate(authUserDTO);

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identities.Any(u => u.IsAuthenticated))
                return RedirectToAction("Index", "Home");
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

                var authUserDTO = mapper.Map<UserDTO, AuthUserDTO>(userDTO);
                await Authentificate(authUserDTO);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Account");
        }
 
        [NonAction]
        private async Task Authentificate(AuthUserDTO authUserDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, authUserDTO.Id.ToString()),                               
                new Claim(ClaimTypes.GivenName, authUserDTO.FirstName),
                new Claim(ClaimTypes.Name, authUserDTO.LastName),
                new Claim(ClaimTypes.Email, authUserDTO.Email)
            };

            if (authUserDTO.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

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
