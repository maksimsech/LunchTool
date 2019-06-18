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

namespace LunchTool.Web.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IConfiguration configuration;
        private LoadDataService dataService;
        private AdministrationService administrationService;
        private IMapper mapper;


        public AdministrationController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            dataService = new LoadDataService(connectionString);
            administrationService = new AdministrationService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginViewModel, UserDTO>();
                cfg.CreateMap<RegisterViewModel, UserDTO>();
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Providers()
        {
            var providers = dataService.GetAllProviders();
            return View(providers);
        }
    }
}
