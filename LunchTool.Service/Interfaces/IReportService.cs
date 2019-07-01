using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces
{
    public interface IReportService
    {
        IEnumerable<UserMonthReportDTO> UserMonthReport(DateTime date, int userId, int providerId);
        IEnumerable<UserProvidersReportDTO> UserProvidersReport(int userId, DateTime fromDate, DateTime toDate);
        IEnumerable<AllUsersReportDTO> AllUsersReport(int providerId, DateTime fromDate, DateTime toDate);
        IEnumerable<UserPageReportDTO> GetUserOrders(int userId, DateTime fromDate, DateTime toDate);
    }
}
