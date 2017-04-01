﻿using System;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Infrastructure.Timing;

namespace Infrastructure.WebApi.Controllers.Dynamic.Binders
{
    /// <summary>
    /// Binds datetime values from api requests to model
    /// </summary>
    public class InfrastructureApiDateTimeBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = value?.ConvertTo(typeof(DateTime?), CultureInfo.CurrentCulture) as DateTime?;

            if (date != null)
            {
                bindingContext.Model = Clock.Normalize(date.Value);
            }
            return true;
        }
    }
}
