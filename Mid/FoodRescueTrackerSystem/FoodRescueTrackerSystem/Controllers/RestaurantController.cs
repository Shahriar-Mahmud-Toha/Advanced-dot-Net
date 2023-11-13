using AutoMapper;
using FoodRescueTrackerSystem.Auth;
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
        public bool confirmPassChecker(string password, string cPassword)
        {
            if (password == cPassword)
            {
                return true;
            }
            TempData["cPassInvalid"] = true;
            return false;
        }

        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                TempData["emptyField"] = true;
                return View(new RestaurantAuthorityDTO { Email = Email, Name = null, Password = null });
            }

            var db = new FoodRescueTrackerSystemEntities();
            if (db.RestaurantAuthorities.Where(d => d.Email == Email && d.Password == Password).SingleOrDefault() != null)
            {
                Session["resAdminAuthLogged"] = true;
                Session["resAdminAuthEmail"] = Email;
                return RedirectToAction("Dashboard");
            }
            TempData["passNotMatched"] = true;
            return View(new RestaurantAuthorityDTO { Email = Email, Name = null, Password = null });
        }
        [HttpGet]
        public ActionResult SignupAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignupAdmin(RestaurantAuthorityDTO n, string cPassword, string LcKey)
        {
            if (!ModelState.IsValid) { return View(n); }
            if (string.IsNullOrEmpty(cPassword)) { return View(n); }
            if (string.IsNullOrEmpty(LcKey))
            {
                TempData["lkeyBlank"] = true;
                return View(n); 
            }
            if (!confirmPassChecker(n.Password, cPassword)) { return View(n); }
            var db = new FoodRescueTrackerSystemEntities();
            if(db.LicenseeKeys.Where(k => k.LcKey == LcKey && k.Role== "adminRes").SingleOrDefault() == null)
            {
                TempData["lkeyInvalid"] = true;
                return View(n);
            }
            db.LicenseeKeys.Remove(db.LicenseeKeys.Where(k => k.LcKey == LcKey && k.Role == "adminRes").Single());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RestaurantAuthorityDTO, RestaurantAuthority>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<RestaurantAuthority>(n);

            db.RestaurantAuthorities.Add(cData);
            db.SaveChanges();
            return RedirectToAction("LoginAdmin");
        }
        [HttpPost]
        [ResAdminLogged]
        public ActionResult logoutAdmin()
        {
            Session["resAdminAuthLogged"] = null;
            return RedirectToAction("LoginAdmin");
        }
        [HttpGet]
        [ResAdminLogged]
        public ActionResult Dashboard()
        {
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
        [ResAdminLogged]
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
            cData.RequestCreatorEmail = Session["resAdminAuthEmail"].ToString();
            db.FoodCollections.Add(cData);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}