﻿using AutoMapper;
using FoodRescueTrackerSystem.Auth;
using FoodRescueTrackerSystem.DTOs;
using FoodRescueTrackerSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodRescueTrackerSystem.Controllers
{
    public class NGOController : Controller
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
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) {
                TempData["emptyField"] = true;
                return View(new NGOAuthorityDTO { Email = Email, Name = null, Password = null }); 
            }

            var db = new FoodRescueTrackerSystemEntities();
            if(db.NGOAuthorities.Where(d=>d.Email ==  Email && d.Password == Password).SingleOrDefault() != null)
            {
                Session["ngoAdminAuthLogged"] = true;
                Session["ngoAdminAuthEmail"] = Email;
                return RedirectToAction("AdminDashboard");
            }
            TempData["passNotMatched"] = true;
            return View(new NGOAuthorityDTO { Email=Email, Name=null, Password=null});
        }
        [HttpGet]
        public ActionResult SignupAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignupAdmin(NGOAuthorityDTO n, string cPassword)
        {
            if (!ModelState.IsValid) { return View(n); }
            if (string.IsNullOrEmpty(cPassword)) { return View(n); }
            if(!confirmPassChecker(n.Password, cPassword)) { return View(n); }

            var db = new FoodRescueTrackerSystemEntities();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NGOAuthorityDTO, NGOAuthority>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<NGOAuthority>(n);

            db.NGOAuthorities.Add(cData);
            db.SaveChanges();
            return RedirectToAction("LoginAdmin");
        }

        [AdminLogged]
        public ActionResult AdminDashboard()
        {
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FoodCollection, FoodCollectionDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<List<FoodCollectionDTO>>(data);
            
            var empData = db.NGOEmployees.ToList();
            var config2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NGOEmployee, NGOEmployeeDTO>();
            });
            var mapper2 = new Mapper(config2);
            var empDataDto = mapper2.Map<List<NGOEmployeeDTO>>(empData);

            ViewBag.empData = empData;

            var empNames= new Dictionary<string, string>(); //empEmail, EmpName
            var resAuthNames= new Dictionary<string, string>(); //resAuthEmail, resAuthName
            var ngoAuthNames = new Dictionary<string, string>(); //ngoAuthEmail, ngoAuthName
            foreach (var items in empData)
            {
                if (!empNames.ContainsKey(items.Email))
                {
                    empNames.Add(items.Email, items.Name);
                }
            }
            foreach (var items in db.RestaurantAuthorities)
            {
                if (!resAuthNames.ContainsKey(items.Email))
                {
                    resAuthNames.Add(items.Email, items.Name);
                }
            }
            foreach (var items in db.NGOAuthorities)
            {
                if (!ngoAuthNames.ContainsKey(items.Email))
                {
                    ngoAuthNames.Add(items.Email, items.Name);
                }
            }
            ViewBag.empNames = empNames;
            ViewBag.ngoAuthNames = ngoAuthNames;
            ViewBag.resAuthNames = resAuthNames;
            return View(cData);
        }
        [HttpPost]
        [AdminLogged]
        public ActionResult Process(int reqId, string DistributorEmail)
        {   
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.Find(reqId);
            var empAuthData = db.NGOEmployees.Find(DistributorEmail);
            if(data != null && empAuthData != null)
            {
                data.ApprovalTime = DateTime.Now;
                data.ApproverEmail = Session["ngoAdminAuthEmail"].ToString();
                data.DistributorEmail = DistributorEmail;
                data.Status = "Accepted";
                db.SaveChanges();
                return RedirectToAction("AdminDashboard");
            }
            TempData["NoDataFoundMsg"] = true;
            return RedirectToAction("AdminDashboard");
        }
        [HttpGet]
        public ActionResult LoginEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginEmployee(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) {
                TempData["emptyField"] = true;
                return View(new NGOEmployeeDTO { Email = Email, Name = null, Password = null }); 
            }

            var db = new FoodRescueTrackerSystemEntities();
            if (db.NGOEmployees.Where(d => d.Email == Email && d.Password == Password).SingleOrDefault() != null)
            {
                Session["ngoEmpLogged"] = true;
                Session["ngEmpEmail"] = Email;
                return RedirectToAction("EmployeeDashboard");
            }
            TempData["passNotMatched"] = true;
            return View(new NGOEmployeeDTO { Email = Email, Name = null, Password = null });
        }
        [HttpGet]
        public ActionResult SignupEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignupEmployee(NGOEmployeeDTO n, string cPassword)
        {
            if (!ModelState.IsValid) { return View(n); }
            if (string.IsNullOrEmpty(cPassword)) { return View(n); }
            if (!confirmPassChecker(n.Password, cPassword)) { return View(n); }

            var db = new FoodRescueTrackerSystemEntities();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NGOEmployeeDTO, NGOEmployee>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<NGOEmployee>(n);

            db.NGOEmployees.Add(cData);
            db.SaveChanges();
            return RedirectToAction("LoginEmployee");
        }
        [EmployeeLogged]
        [HttpGet]
        public ActionResult EmployeeDashboard()
        {
            string email = Session["ngEmpEmail"].ToString();
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.Where(d=>d.DistributorEmail==email).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FoodCollection, FoodCollectionDTO>();
            });
            var mapper = new Mapper(config);
            var cData = mapper.Map<List<FoodCollectionDTO>>(data);
            return View(cData);
        }
        [EmployeeLogged]
        [HttpPost]
        public ActionResult Collected(int reqId)
        {
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.Find(reqId);
            if (data != null)
            {
                data.Status = "Collected";
                data.CollectionTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("EmployeeDashboard");
            }
            TempData["NoDataFoundMsg"] = true;
            return RedirectToAction("EmployeeDashboard");
        }
        [EmployeeLogged]
        [HttpPost]
        public ActionResult Distributed(int reqId)
        {
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.Find(reqId);
            if (data != null)
            {
                data.Status = "Distributed";
                data.DistributionTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("EmployeeDashboard");
            }
            TempData["NoDataFoundMsg"] = true;
            return RedirectToAction("EmployeeDashboard");
        }
    }
}