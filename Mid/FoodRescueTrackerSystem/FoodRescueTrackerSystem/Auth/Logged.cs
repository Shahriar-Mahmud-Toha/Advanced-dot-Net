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
            if ((httpContext.Session["ngoAdminAuthLogged"] != null) && (bool)(httpContext.Session["ngoAdminAuthLogged"])) return true;
            return false;
        }
    }
    public class EmployeeLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if((httpContext.Session["ngoEmpLogged"] != null) && (bool)(httpContext.Session["ngoEmpLogged"])) return true;
            return false;
        }
    }
    public class ResAdminLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if ((httpContext.Session["resAdminAuthLogged"] != null) && (bool)(httpContext.Session["resAdminAuthLogged"])) return true;
            return false;
        }
    }
}