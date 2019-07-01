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
    public class ReportController : Controller
    {
        private readonly IConfiguration configuration;
        private ILoadDataService dataService;
        private IReportService reportService;
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
                cfg.CreateMap<UserProvidersReportDTO, UserProvidersReportViewModel>();
                cfg.CreateMap<AllUsersReportDTO, AllUsersReportViewModel>();
                cfg.CreateMap<UserPageReportDTO, UserPageReportViewModel>();
            }).CreateMapper();
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

        [Authorize(Roles = "Administrator")]
        public IActionResult UserProvidersReport()
        {
            var usersDTO = dataService.Users.GetAll();
            var usersViewModel = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(usersDTO);
            return View(usersViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult UserProvidersReport(int UserId, DateTime FromDate, DateTime ToDate)
        {
            var reportDTO = reportService.UserProvidersReport(UserId, FromDate, ToDate);
            var reportViewModel = mapper.Map<IEnumerable<UserProvidersReportDTO>, IEnumerable<UserProvidersReportViewModel>>(reportDTO);
            return PartialView("~/Views/Shared/_UserProvidersReport.cshtml", reportViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AllUsersReport()
        {
            var providersDTO = dataService.Providers.GetAll();
            var providersViewModel = mapper.Map<IEnumerable<ProviderDTO>, IEnumerable<ProviderViewModel>>(providersDTO);
            return View(providersViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AllUsersReport(int ProviderId, DateTime FromDate, DateTime ToDate)
        {
            var reportDTO = reportService.AllUsersReport(ProviderId, FromDate, ToDate);
            var reportViewModel = mapper.Map<IEnumerable<AllUsersReportDTO>, IEnumerable<AllUsersReportViewModel>>(reportDTO);
            return PartialView("~/Views/Shared/_AllUsersReport.cshtml", reportViewModel);
        }

        public IActionResult UserReport()
        {
            var date = DateTime.Today;
            var (startOfWeek, endOfWeek) = GetStartAndEndOfWeek(date);
            return View((startOfWeek, endOfWeek));
        }

        [HttpPost]
        public IActionResult UserReport(DateTime FromDate, DateTime ToDate)
        {
            var userId = int.Parse(User.Identity.Name);
            var reportDTO = reportService.GetUserOrders(userId, FromDate, ToDate);
            var reportViewModel = mapper.Map<IEnumerable<UserPageReportDTO>, IEnumerable<UserPageReportViewModel>>(reportDTO);  
            return PartialView("~/Views/Shared/_UserReport.cshtml", reportViewModel);
        }

        [NonAction]
        private (DateTime startOfWeek, DateTime endOfWeek) GetStartAndEndOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = date.AddDays(-1 * diff).Date;
            var endOfWeek = startOfWeek.AddDays(6).Date;
            return (startOfWeek, endOfWeek);
        }
    }
}
