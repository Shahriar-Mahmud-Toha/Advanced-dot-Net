using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodRescueTrackerSystem.Auth
{
    public class AdminLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["ngoAdminAuthLogged"] != null) return true;
            return false;
        }
    }
    public class EmployeeLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["ngoEmpLogged"] != null) return true;
            return false;
        }
    }
    public class ResAdminLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["resAdminAuthLogged"] != null) return true;
            return false;
        }
    }
}