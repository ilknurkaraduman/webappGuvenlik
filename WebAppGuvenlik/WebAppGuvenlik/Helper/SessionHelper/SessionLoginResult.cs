using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Helper.SessionHelper
{
    public class SessionLoginResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        public SessionLoginResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}