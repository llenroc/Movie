﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Web.Web.MultiTenancy
{
    public interface IWebMultiTenancyConfiguration
    {
        string DomainFormat { get; set; }
    }
}
