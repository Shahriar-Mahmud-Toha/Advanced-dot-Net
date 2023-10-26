using AutoMapper;
using InventoryMS.Auth;
using InventoryMS.DTOs;
using InventoryMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryMS.Controllers
{
    public class AuthorityController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AuthorityDTO a)
        {
            var db = new InventoryMSEntities();
            var spData = (from data in db.Authorities
                          where data.Email == a.Email && data.Password == a.Password
                          select data).SingleOrDefault();
            if (spData != null)
            {
                Session["AdminloggedIn"] = true;
                Session["adminEmail"] = a.Email;
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [AdminLogged]
        public ActionResult Dashboard()
        {
            var db = new InventoryMSEntities();
            var data = db.ProductsCustomers.ToList();
            return View(data);
        }
        [AdminLogged]
        public ActionResult Process(int id)
        {
            var db = new InventoryMSEntities();
            var data = db.ProductsCustomers.Find(id);
            data.OrderStatus = "Processing";
            data.Product.Quantity -= data.Count;
            db.SaveChanges();
            TempData["processed"] = true;
            return RedirectToAction("Dashboard");
        }
        [AdminLogged]
        public ActionResult Decline(int id)
        {
            var db = new InventoryMSEntities();
            var data = db.ProductsCustomers.Find(id);
            data.OrderStatus = "Declined";
            db.SaveChanges();
            TempData["declined"] = true;
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        [AdminLogged]
        public ActionResult Logout()
        {
            Session["loggedIn"] = null;
            return RedirectToAction("Login");
        }
    }
}