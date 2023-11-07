using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            ViewBag.name = form["name"];
            ViewBag.id = form["id"];
            ViewBag.gender = form["gender"];
            ViewBag.profession = form["profession"];
            ViewBag.hobbies = form["hobbies"];
            ViewBag.status = true;
            TempData["success_msg"] = true;
            return View();
        }
    }
}