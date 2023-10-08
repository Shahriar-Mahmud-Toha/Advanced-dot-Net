using FormValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormValidation.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Person p)
        {
            if (ModelState.IsValid)
            {
                bool res = emailAndIdFormatMatched(p.Email, p.ID);
                if (res)
                {
                    TempData["pName"] = p.Name;
                    TempData["pUsername"] = p.Username;
                    TempData["pEmail"] = p.Email;
                    TempData["pID"] = p.ID;
                    TempData["pPassword"] = p.Password;
                    TempData["pDob"] = p.Dob;
                    TempData["emIdMt"] = res;
                    TempData["tr1"] = new Person();
                    return RedirectToAction("Dashboard");
                }
                TempData["emIdMt"] = res;
            }
            return View(p);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        bool emailAndIdFormatMatched(string email, string id)
        {
            bool flag = false;

            string[] emId = email.Split('@');
            if (emId[0] == id)
            {
                flag = true;
            }

            return flag;
        }
    }
}