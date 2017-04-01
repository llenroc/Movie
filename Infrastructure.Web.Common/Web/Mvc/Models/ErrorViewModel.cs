﻿using System;
using Infrastructure.Web.Models;

namespace Infrastructure.Web.Mvc.Models
{
    public class ErrorViewModel
    {
        public ErrorInfo ErrorInfo { get; set; }

        public Exception Exception { get; set; }

        public ErrorViewModel()
        {

        }

        public ErrorViewModel(ErrorInfo errorInfo, Exception exception = null)
        {
            ErrorInfo = errorInfo;
            Exception = exception;
        }
    }
}
