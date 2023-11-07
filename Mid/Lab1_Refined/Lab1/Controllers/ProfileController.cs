using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult References()
        {
            return View();
        }
        public ActionResult Bio()
        {
            ViewBag.projectInBio = "xyz, jhu, lko";
            ViewData["skills"] = "mn,op,bv,ng";
            return View();
        }
        public ActionResult Education()
        {
            return View();
        }
        public ActionResult Projects()
        {
            return View();
        }
    }
}