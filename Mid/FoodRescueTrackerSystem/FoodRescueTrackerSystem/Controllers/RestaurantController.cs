using AutoMapper;
using FoodRescueTrackerSystem.DTOs;
using FoodRescueTrackerSystem.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodRescueTrackerSystem.Controllers
{
    public class RestaurantController : Controller
    {
        [HttpGet]
        public ActionResult Dashboard()
        {
            Session["resAuthEmail"] = "r@mail.com"; //must be removed this line

            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FoodCollection, FoodCollectionDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<List<FoodCollectionDTO>>(data);
            return View(cData);
        }
        [HttpPost]
        public ActionResult Dashboard(FoodCollectionDTO f)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FoodCollectionDTO, FoodCollection>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<FoodCollection>(f);

            var db = new FoodRescueTrackerSystemEntities();
            cData.RequestTime = DateTime.Now;
            cData.Status = "Requested";
            cData.RequestCreatorEmail = Session["resAuthEmail"].ToString();
            db.FoodCollections.Add(cData);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}