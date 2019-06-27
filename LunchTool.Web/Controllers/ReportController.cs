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

            }).CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeReport(DateTime ReportDate, int dateType, int reportType)
        {
            if (ModelState.IsValid)
            {
                switch ((ReportType)reportType)
                {
                    case ReportType.Provider:
                        reportService.Make(DateTime.Now.Date);
                        break;
                    case ReportType.User:

                        break;
                }
            }
            return Content("ops");
        }
    }
}
