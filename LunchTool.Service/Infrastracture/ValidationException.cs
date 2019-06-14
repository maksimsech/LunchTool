using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Infrastracture
{
    public class ValidationException: Exception
    {
        public string Property { get; protected set; }
        public ValidationException(string property, string message): base(message)
        {
            Property = property;
        }
    }
}
