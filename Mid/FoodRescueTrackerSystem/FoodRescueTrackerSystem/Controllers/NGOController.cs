using AutoMapper;
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
        public ActionResult AdminDashboard()
        {
            Session["ngoAuthEmail"] = "n@mail.com"; //must be removed this line

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
        public ActionResult Process(int reqId, string DistributorEmail)
        {   
            var db = new FoodRescueTrackerSystemEntities();
            var data = db.FoodCollections.Find(reqId);
            var empAuthData = db.NGOEmployees.Find(DistributorEmail);
            if(data != null && empAuthData != null)
            {
                data.ApprovalTime = DateTime.Now;
                data.ApproverEmail = Session["ngoAuthEmail"].ToString();
                data.DistributorEmail = DistributorEmail;
                data.Status = "Accepted";
                db.SaveChanges();
                return RedirectToAction("AdminDashboard");
            }
            TempData["NoDataFoundMsg"] = true;
            return RedirectToAction("AdminDashboard");
        }
        [HttpGet]
        public ActionResult EmployeeDashboard()
        {
            Session["ngEmpAuthEmail"] = "ne@mail.com"; //must be removed this line

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