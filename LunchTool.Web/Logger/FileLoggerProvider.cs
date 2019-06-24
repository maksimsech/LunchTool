using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string filePath;

        public FileLoggerProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(filePath);

        public void Dispose()
        {

        }
    }
}
