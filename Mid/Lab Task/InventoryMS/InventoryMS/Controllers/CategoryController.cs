using AutoMapper;
using InventoryMS.DTOs;
using InventoryMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryMS.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            var db = new InventoryMSEntities();
            var data = db.Categories.ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<List<CategoryDTO>>(data);
            return View(cData);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CategoryDTO p)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryDTO, Category>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<Category>(p);

            var db = new InventoryMSEntities();
            db.Categories.Add(cData);
            db.SaveChanges();
            TempData["successCtCr"] = true;
            return View();
        }
        public ActionResult Edit(int id)
        {
            var db = new InventoryMSEntities();
            var currData = (from data in db.Categories
                            where data.Id == id
                            select data).SingleOrDefault();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<CategoryDTO>(currData);
            return View(cData);
        }
        [HttpPost]
        public ActionResult Edit(CategoryDTO p)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CategoryDTO, Category>();
                });
                var mapper = new Mapper(config);
                var cData = mapper.Map<Category>(p);

                var db = new InventoryMSEntities();
                var currData = db.Categories.Find(cData.Id);
                currData.Name = cData.Name;
                db.SaveChanges();
                TempData["successCtEdit"] = true;
                return RedirectToAction("Index");
            }
            return View(p);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new InventoryMSEntities();
            var currData = from data in db.Categories
                           where data.Id == id
                           select data;
            db.Categories.RemoveRange(currData);
            var currStd = db.Categories.Find(id);
            db.Categories.Remove(currStd);
            db.SaveChanges();
            TempData["catRemoved"] = true;
            return RedirectToAction("Index");
        }
    }
}