using InventoryMS.EF;
using InventoryMS.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace InventoryMS.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            var db = new InventoryMSEntities();
            var data = db.Products.ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<List<ProductDTO>>(data);
            return View(cData);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductDTO p)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDTO, Product>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<Product>(p);
            
            var db = new InventoryMSEntities();
            db.Products.Add(cData);
            db.SaveChanges();
            TempData["successCr"] = true;
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var db = new InventoryMSEntities();
            var currData = (from data in db.Products
                            where data.Id == Id
                            select data).SingleOrDefault();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<ProductDTO>(currData);
            return View(cData);
        }
        [HttpPost]
        public ActionResult Edit(ProductDTO p)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProductDTO, Product>();
                });
                var mapper = new Mapper(config);
                var cData = mapper.Map<Product>(p);

                var db = new InventoryMSEntities();
                var currData = db.Products.Find(cData.Id);
                currData.Name = cData.Name;
                currData.CtId = cData.CtId;
                currData.Price = cData.Price;
                db.SaveChanges();
                TempData["successPrEdit"] = true;
                return RedirectToAction("Index");
            }
            return View(p);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var db = new InventoryMSEntities();
            
            var relData = from r in db.ProductsCustomers
                          where r.PdId == Id
                          select r;
            db.ProductsCustomers.RemoveRange(relData);

            var currData = from data in db.Products
                           where data.Id == Id
                           select data;
            db.Products.RemoveRange(currData);
            //db.CoursesStudents.RemoveRange(db.CoursesStudents.Where(s => s.Id == id)); //using lambda expression
            db.SaveChanges();
            TempData["prodRemoved"] = true;
            return RedirectToAction("Index");
        }
       
    }
}