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
using LunchTool.Web.Models;

namespace LunchTool.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportController : Controller
    {
        private readonly IConfiguration configuration;
        private ILoadDataService dataService;
        private ReportService reportService;
        private IMapper mapper;

        public ReportController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            dataService = new LoadDataService(connectionString);
            reportService = new ReportService(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProviderDTO, ProviderViewModel>();
                cfg.CreateMap<UserMonthReportDTO, UserMonthReportViewModel>();
                cfg.CreateMap<UserDTO, UserViewModel>();
            }).CreateMapper();
        }

        public IActionResult Index()
        {
            var month = 6;
            var year = 2019;
            var date = DateTime.Now;
            var fromDate = new DateTime(year, month, 25, 0, 0, 0);
            var toDate = new DateTime(year, month, 28, 0, 0, 0);
            var report = reportService.UserMonthReport(date, 1, -1);
            var report2 = reportService.UserProvidersReport(1, fromDate, toDate);
            var report3 = reportService.AllUsersReport(2, fromDate, toDate);
            var report4 = reportService.GetUserOrders(1, fromDate, toDate);
            int a = 1;
            return Content("hi");
        }

        [HttpPost]
        public IActionResult MakeReport(DateTime ReportDate, int dateType, int reportType)
        {
            return Content("ops");
        }

        public IActionResult UserMonthReport()
        {
            var providersDTO = dataService.Providers.GetAll();
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            return View(providersViewModel);
        }

        [HttpPost]
        public IActionResult UserMonthReport(int ProviderId, int Month, int Year)
        {
            var userId = int.Parse(User.Identity.Name);
            var date = new DateTime(Year, Month, 1, 1, 1, 1);
            var reportDTO = reportService.UserMonthReport(date, userId, ProviderId);
            var reportViewModel = mapper.Map<IEnumerable<UserMonthReportDTO>, IEnumerable<UserMonthReportViewModel>>(reportDTO);
            return PartialView("~/Views/Shared/_UserMonthReport.cshtml", reportViewModel);
        }

        public IActionResult UserProvidersReport(int userId, DateTime fromDate, DateTime toDate)
        {
            var usersDTO = dataService.Users.GetAll();
            var usersViewModel = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(usersDTO);
            return View(usersViewModel);
        }
    }
}
