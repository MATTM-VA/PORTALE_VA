using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Membership;

namespace VAPortale.Code
{
    public class VAController : Controller
    {
        protected virtual new VAPrincipal User
        {
            get
            {
                return (VAPrincipal)HttpContext.User;
            }
        }
    }
}