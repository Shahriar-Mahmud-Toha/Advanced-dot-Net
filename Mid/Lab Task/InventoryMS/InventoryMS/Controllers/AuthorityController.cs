using AutoMapper;
using InventoryMS.Auth;
using InventoryMS.DTOs;
using InventoryMS.EF;
using Microsoft.Ajax.Utilities;
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
            //var db = new InventoryMSEntities();
            //var data = db.OrderDetails.ToList();
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<OrderDetail, OrderDetailDTO>();
            //});
            //var mapper = new Mapper(config);
            //var cData = mapper.Map<List<OrderDetailDTO>>(data);
            //return View(data);

            var db = new InventoryMSEntities();
            var orderData = db.Orders.ToList();
            var orders = db.OrderDetails.ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = new Mapper(config);
            var ordData = mapper.Map<List<OrderDTO>>(orderData);
            ViewBag.OrderData = ordData;
            var ctNames = new Dictionary<int, string>(); //pdId, CatName
            var pdNames = new Dictionary<int, string>(); // pdId, PdName
            var pdStQty = new Dictionary<int, int>(); // pdId, PdStQty
            foreach (var items in orders)
            {
                if (!pdNames.ContainsKey(items.ProductId))
                {
                    pdNames.Add(items.ProductId, items.Product.Name);
                }
                if (!pdStQty.ContainsKey(items.ProductId))
                {
                    pdStQty.Add(items.ProductId, items.Product.Quantity);
                }
                if (!ctNames.ContainsKey(items.ProductId))
                {
                    ctNames.Add(items.ProductId, items.Product.Category.Name);
                }
            }
            ViewBag.ctNames = ctNames;
            ViewBag.pdNames = pdNames;
            ViewBag.pdStQty = pdStQty;
            var config2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDetail, OrderDetailDTO>();
            });
            var mapper2 = new Mapper(config2);
            var cData = mapper2.Map<List<OrderDetailDTO>>(orders);
            return View(cData);
        }
        [AdminLogged]
        public ActionResult Process(int OrId)
        {
            var db = new InventoryMSEntities();
            var data = db.Orders.Find(OrId);
            data.Status = "Processing";
            var acData = from d in db.OrderDetails
                         where d.OrId == OrId
                         select d;
            foreach(var item in acData)
            {
                item.Product.Quantity -= item.OrderedQuantity;
            }
            db.SaveChanges();
            TempData["processed"] = true;
            return RedirectToAction("Dashboard");
        }
        [AdminLogged]
        public ActionResult Decline(int OrId)
        {
            var db = new InventoryMSEntities();
            var data = db.Orders.Find(OrId);
            data.Status = "Declined";
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