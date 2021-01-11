using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using VAPortale.Code;
using VALib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using VAPortale.Models;
using System.Configuration;

namespace VAPortale.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RemoveScriptAttribute : ActionFilterAttribute
    {
        private string pattern = ConfigurationManager.AppSettings["ReplaceTextPattern"] ?? "[^0-9a-zèòàùì ]+" ;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            var valueProviderResult = filterContext.Controller.ValueProvider.GetValue("Testo");

            if (valueProviderResult != null)
            {
                dynamic model = filterContext.ActionParameters["model"];
                if (model == null)
                {
                    return;
                }

                model.Testo = Regex.Replace(valueProviderResult.AttemptedValue.ToString(), pattern, "", RegexOptions.IgnoreCase);
            }
        }
    }
}