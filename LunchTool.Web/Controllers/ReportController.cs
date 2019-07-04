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
using IronPdf;
using NickBuhro.Translit;
using System.IO;

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

            TempData["Month"] = Month < 10 ? "0" + Month.ToString() : Month.ToString();
            TempData["Year"] = Year.ToString();
            if(ProviderId == -1)
            {
                TempData["ProviderName"] = "Все";
            }
            else
            {
                var providerName = dataService.Providers.Get(p => p.Id == ProviderId).Select(p => p.Name).FirstOrDefault();
                TempData["ProviderName"] = providerName;
            }
            var user = dataService.Users.Get(u => u.Id == userId).FirstOrDefault();
            var userName = $"{user.LastName} {user.FirstName} {user.Patronymic?? ""}";
            TempData["UserName"] = userName;

            var reportDTO = reportService.UserMonthReport(date, userId, ProviderId);
            if (reportDTO.All(r => r.OrderCount == 0))
            {
                return Content("");
            }
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
            string fromDay = FromDate.Day < 10 ? "0" + FromDate.Day.ToString() : FromDate.Day.ToString();
            string fromMonth = FromDate.Month < 10 ? "0" + FromDate.Month.ToString() : FromDate.Month.ToString();
            string toDay = ToDate.Day < 10 ? "0" + ToDate.Day.ToString() : ToDate.Day.ToString();
            string toMonth = ToDate.Month < 10 ? "0" + ToDate.Month.ToString() : ToDate.Month.ToString();
            TempData["FromDate"] = $"{fromDay}.{fromMonth}.{FromDate.Year} г.";
            TempData["ToDate"] = $"{toDay}.{toDay}.{ToDate.Year} г.";
            var reportDTO = reportService.UserProvidersReport(UserId, FromDate, ToDate);
            if (reportDTO.Count() == 0)
            {
                return Content("");
            }
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
            if (ProviderId == -1)
            {
                TempData["ProviderName"] = "Все";
            }
            else
            {
                var providerName = dataService.Providers.Get(p => p.Id == ProviderId).Select(p => p.Name).FirstOrDefault();
                TempData["ProviderName"] = providerName;
            }
            string fromDay = FromDate.Day < 10 ? "0" + FromDate.Day.ToString() : FromDate.Day.ToString();
            string fromMonth = FromDate.Month < 10 ? "0" + FromDate.Month.ToString() : FromDate.Month.ToString();
            string toDay = ToDate.Day < 10 ? "0" + ToDate.Day.ToString() : ToDate.Day.ToString();
            string toMonth = ToDate.Month < 10 ? "0" + ToDate.Month.ToString() : ToDate.Month.ToString();
            TempData["FromDate"] = $"{fromDay}.{fromMonth}.{FromDate.Year} г.";
            TempData["ToDate"] = $"{toDay}.{toDay}.{ToDate.Year} г.";
            var reportDTO = reportService.AllUsersReport(ProviderId, FromDate, ToDate);
            if(reportDTO.Count() == 0)
            {
                return Content("");
            }
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
            if (reportDTO.Count() == 0)
            {
                return Content("");
            }
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

        [HttpPost]
        public string MakePdf(string html, string userName)
        {
            var option = new PdfPrintOptions
            {
                CustomCssUrl = new Uri(Path.Combine(Environment.CurrentDirectory, @"wwwroot\lib\bootstrap\dist\css\bootstrap.css")),
                MarginBottom = 5,
                MarginLeft = 5,
                MarginRight = 5,
                MarginTop = 5
            };
            var renderer = new HtmlToPdf(option);
            var doc = renderer.RenderHtmlAsPdf(html);
            var tempPath = Path.GetTempPath();
            var translitUserName = Transliteration.CyrillicToLatin(userName);
            translitUserName = translitUserName.Replace(' ', '-');
            var fileName = $"Otchet-{translitUserName}-{DateTime.Now.ToString("yyyy-MM-dd")}.pdf";
            var fullPath = Path.Combine(tempPath, fileName);
            System.IO.File.WriteAllBytes(fullPath, doc.BinaryData);
            return fileName;
        }

        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var fullPath = Path.Combine(Path.GetTempPath(), fileName);
            var memorySteam = new MemoryStream();
            try
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    await fileStream.CopyToAsync(memorySteam);
                }
            }
            catch (Exception e){
                return Content("Создайте заново отчет.");
            }
            System.IO.File.Delete(fullPath);
            memorySteam.Position = 0;
            return File(memorySteam, "application/pdf;");
        }
    }
}
