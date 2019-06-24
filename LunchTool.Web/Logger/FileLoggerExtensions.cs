using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.Logger
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filePath)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(filePath));
            return loggerFactory;
        }
    }
}
