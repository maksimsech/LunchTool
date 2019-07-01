using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
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
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private Service.Interfaces.IAuthenticationService authentificationService;
        private ILoadDataService dataService;
        private IMapper mapper;


        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            authentificationService = new Service.Implementation.AuthenticationService(connectionString);
            dataService = new LoadDataService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginViewModel, UserDTO>();
                cfg.CreateMap<RegisterViewModel, UserDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
                cfg.CreateMap<UserViewModel, UserDTO>();
            }).CreateMapper();
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name == null)
                return RedirectToAction("Login", "Account");
            var userId = int.Parse(User.Identity.Name);
            var userDTO = dataService.Users.Get(u => u.Id == userId).FirstOrDefault();
            var userViewModel = mapper.Map<UserDTO, UserViewModel>(userDTO);
            return View(userViewModel);
        }

        [Authorize]
        public IActionResult ChangeInfo()
        {
            var userId = int.Parse(User.Identity.Name);
            var userDTO = dataService.Users.Get(u => u.Id == userId).FirstOrDefault();
            var userViewModel = mapper.Map<UserDTO, UserViewModel>(userDTO);
            return View(userViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeInfo(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.Identity.Name);
                var find = dataService.Users.Get(u => u.Id == userId).FirstOrDefault();
                if (find == null)
                {
                    return Content("Пользователь не найден");
                }
                userViewModel.Id = userId;
                userViewModel.Password = find.Password;
                userViewModel.IsAdmin = find.IsAdmin;
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                authentificationService.ChangeInfo(userDTO);
                return RedirectToAction("Index", "Account");
            }
            return Content("Проверьте данные");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.Identity.Name);
                var userEmail = dataService.Users.Get(u => u.Id == userId).Select(u => u.Email).FirstOrDefault();

                var oldPassword = GetHash(userEmail, changePasswordViewModel.OldPassword);
                if (authentificationService.CheckPassword(userId, oldPassword))
                {
                    var newPassword = GetHash(userEmail, changePasswordViewModel.NewPassword);
                    authentificationService.ChangePassword(userId, newPassword);
                    return RedirectToAction("Index", "Account");
                }
            }
            return Content("Проверьте данные");
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
                userDTO.Password = GetHash(loginViewModel.Email, loginViewModel.Password);
                if (authentificationService.CheckLogin(userDTO))
                {
                    var authUserDTO = authentificationService.GetAuthUserDTO(userDTO);
                    await Authentificate(authUserDTO);

                    return RedirectToRoute("home");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identities.Any(u => u.IsAuthenticated))
                return RedirectToRoute("home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<RegisterViewModel, UserDTO>(registerViewModel);
                userDTO.Password = GetHash(registerViewModel.Email, registerViewModel.Password);
                if (authentificationService.IsRegistered(userDTO))
                {
                    ModelState.AddModelError("", "Пользователь уже существует");
                    return RedirectToAction("Register", "Account");
                }
                userDTO.IsAdmin = false;
                authentificationService.Register(userDTO);

                var authUserDTO = mapper.Map<UserDTO, AuthUserDTO>(userDTO);
                await Authentificate(authUserDTO);

                return RedirectToRoute("home");
            }

            return RedirectToAction("Register", "Account");
        }

        [NonAction]
        private string GetHash(string email, string password)
        {
            byte[] result;
            using(var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(email + password);
                result = sha256.ComputeHash(bytes);
            }
            return BitConverter.ToString(result);
        }
 
        [NonAction]
        private async Task Authentificate(AuthUserDTO authUserDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, authUserDTO.Id.ToString()),                               
                new Claim(ClaimTypes.GivenName, authUserDTO.FirstName),
                new Claim(ClaimTypes.NameIdentifier, authUserDTO.LastName),
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
            return RedirectToAction("Login", "Account");
        }
        
    }
}
