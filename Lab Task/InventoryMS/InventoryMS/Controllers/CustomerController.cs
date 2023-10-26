using AutoMapper;
using InventoryMS.Auth;
using InventoryMS.DTOs;
using InventoryMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace InventoryMS.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(CustomerDTO c)
        {
            var db = new InventoryMSEntities();
            var spData = (from data in db.Customers
                         where data.Email == c.Email && data.Password == c.Password
                         select data).SingleOrDefault();
            if (spData != null)
            {
                Session["loggedIn"] = true;
                Session["CusEmail"] = c.Email;
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [Logged]
        public ActionResult Dashboard()
        {
            if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
            {
                var db = new InventoryMSEntities();
                var data = db.Categories.ToList();
                
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Category, CategoryDTO>();
                });
                var mapper = new Mapper(config);
                var cData = mapper.Map<List<CategoryDTO>>(data);

                //var productList = db.Products.Select(p => new ProductDTO
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    CtId = p.CtId
                //}).ToList();
                
                var productDb = db.Products.ToList();
                var configPd = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDTO>();
                });
                var mapperPd = new Mapper(configPd);
                var pData = mapperPd.Map<List<ProductDTO>>(productDb);

                ViewBag.Products = JsonConvert.SerializeObject(pData.ToList());

                //ViewBag.Products = JsonConvert.SerializeObject(convert(productDb).ToList());

                return View(cData);
            }
            return RedirectToAction("Login");
        }
        //public ActionResult addToCart(int cartPd)
        //{
        //    if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
        //    {
        //        #region
        //        if (Request.Cookies["pdList"] != null)
        //        {
        //            var cookie = Request.Cookies["pdList"];
        //            var existingValue = cookie.Value;
        //            var newValue = existingValue.ToString() + "," + cartPd.ToString() + ",";
        //            //c["pdList"] = Request.Cookies["pdList"].Value.ToString() + "," + cartPd.ToString() + ",";
        //            cookie.Value = newValue;
        //            Response.SetCookie(cookie);
        //        }
        //        else
        //        {
        //            HttpCookie c = new HttpCookie("pdList");
        //            c["pdList"] = cartPd.ToString();
        //            Response.SetCookie(c);
        //        }
        //        #endregion
        //        return RedirectToAction("Dashboard");
        //    }
        //    return RedirectToAction("Login");
        //}
        //public ActionResult addToCart(int cartPd)
        //{
        //    if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
        //    {
        //        var pdList = new List<int>();

        //        if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
        //        {
        //            pdList = JsonConvert.DeserializeObject<List<int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
        //        }

        //        pdList.Add(cartPd);
        //        var newCookie = new HttpCookie("pdList", HttpUtility.UrlEncode(JsonConvert.SerializeObject(pdList)));
        //        Response.Cookies.Add(newCookie);

        //        return RedirectToAction("Dashboard");
        //    }
        //    return RedirectToAction("Login");
        //}
        [Logged]
        public ActionResult addToCart(int cartPd, int selectedCategory)
        {
            if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
            {
                var pdDictionary = new Dictionary<int, int>();

                if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
                {
                    pdDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
                }

                if (pdDictionary.ContainsKey(cartPd))
                {
                    pdDictionary[cartPd] += 1; // Increment the count for an existing product
                }
                else
                {
                    pdDictionary.Add(cartPd, 1); // Add the product to the dictionary with a count of 1
                }

                var newCookie = new HttpCookie("pdList", HttpUtility.UrlEncode(JsonConvert.SerializeObject(pdDictionary)));
                Response.Cookies.Add(newCookie);
                TempData["selectedCategory"] = selectedCategory;
                TempData["successAddToCart"] = true;
                return RedirectToAction("Dashboard", new { selectedCategory });
            }

            return RedirectToAction("Login");
        }
        [Logged]
        [HttpGet]
        public ActionResult showCart()
        {
                if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
                {
                    var pdDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
                    var productList = new List<Product>();
                    var db = new InventoryMSEntities();
                    foreach (var item in pdDictionary)
                    {
                        var data = db.Products.Find(item.Key);
                        if(data != null)
                        {
                            productList.Add(data);
                        }
                    }
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Product, ProductDTO>();
                    });
                    var mapper = new Mapper(config);
                    var cData = mapper.Map<List<ProductDTO>>(productList);
                    ViewBag.pdDictionary = pdDictionary;
                    return View(cData);
                }
                TempData["noItemInCart"] = true;
                return RedirectToAction("Dashboard");
        }
        [Logged]
        [HttpPost]
        public ActionResult showCart(CustomerDTO c)
        {
            if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
            {
                if (ModelState.IsValid)
                {
                    var pdDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
                    var products = new List<ProductsCustomer>();

                    var db = new InventoryMSEntities();
                    var data = db.Customers.Find(Session["CusEmail"]);
                    data.Address = c.Address;
                    foreach (var item in pdDictionary)
                    {
                        var pc = new ProductsCustomer();
                        pc.PdId = item.Key;
                        pc.CusEmail = Session["CusEmail"].ToString();
                        pc.Count = item.Value;
                        pc.OrderStatus = "Ordered";
                        pc.OrderTime = DateTime.Now;
                        products.Add(pc);
                    }
                    foreach (var item in products)
                    {
                        db.ProductsCustomers.Add(item);
                    }
                    db.SaveChanges();
                    
                    HttpCookie cartItems = new HttpCookie("pdList");
                    cartItems.Expires = DateTime.Now.AddDays(-1); 
                    Response.Cookies.Add(cartItems);
                    TempData["successOrder"] = true;
                    return RedirectToAction("Dashboard");
                }
                ViewBag.Address = c.Address;
                return View();
            }
            return RedirectToAction("Dashboard");
        }
        [Logged]
        public ActionResult showOrders()
        {
            var db = new InventoryMSEntities();
            string cusEmail = Session["CusEmail"].ToString();
            var data = (from d in db.ProductsCustomers
                       where d.CusEmail == cusEmail
                       select d).ToList();
            #region: Using Viewbag
            //var pdList = new List<Product>();
            //var ctNames = new Dictionary<int, string>();
            //var pdNames = new Dictionary<int, string>();
            //var pdPrices = new Dictionary<int, int>();
            //foreach (var p in data)
            //{
            //    var pd = db.Products.Find(p.PdId);
            //    pdList.Add(pd);
            //    string catName = db.Categories.Find(pd.CtId).Name;

            //    if (!ctNames.ContainsKey(p.PdId))
            //    {
            //        ctNames.Add(p.PdId, catName);
            //    }
            //    if (!pdNames.ContainsKey(p.PdId))
            //    {
            //        pdNames.Add(p.PdId, pd.Name);
            //    }
            //    if (!pdPrices.ContainsKey(p.PdId))
            //    {
            //        pdPrices.Add(p.PdId, pd.Price);
            //    }
            //}
            //ViewBag.ctNames = ctNames;
            //ViewBag.pdNames = pdNames;
            //ViewBag.pdPrices = pdPrices;
            
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<ProductsCustomer, ProductsCustomerDTO>();
            //});
            //var mapper = new Mapper(config);
            //var cData = mapper.Map<List<ProductsCustomerDTO>>(data);
            #endregion:Using Viewbag
            
            return View(data);
        }
        //public ActionResult showCart()
        //{
        //    if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
        //    {
        //        if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
        //        {
        //            var pdList = new List<int>();
        //            pdList = JsonConvert.DeserializeObject<List<int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
        //            var db = new InventoryMSEntities();
        //            List<Product> prodLs = new List<Product>();
        //            foreach(var x in pdList)
        //            {
        //                var data = (from d in db.Products
        //                            where d.Id == x
        //                            select d).SingleOrDefault();
        //                prodLs.Add(data);
        //            }
        //            var config = new MapperConfiguration(cfg =>
        //            {
        //                cfg.CreateMap<Product, ProductDTO>();
        //            });
        //            var mapper = new Mapper(config);
        //            var cData = mapper.Map<List<ProductDTO>>(prodLs);

        //            return View(cData);
        //        }
        //        return RedirectToAction("Dashboard");
        //    }
        //    return RedirectToAction("Login");
        //}
        //public ActionResult showCart()
        //{
        //    if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
        //    {
        //        if (Request.Cookies["pdList"] != null)
        //        {
        //            var fullVal = Request.Cookies["pdList"].Value.ToString().Split('=');
        //            var val = fullVal[1];
        //            var temp = val.Split(',');
        //            var db = new InventoryMSEntities();
        //            List<Product> prodLs = new List<Product>();
        //            for (int i = 0; i < temp.Length; i++)
        //            {

        //                if (temp[i] != "")
        //                {
        //                    int tempId = Int32.Parse(temp[i]);
        //                    var data = (from d in db.Products
        //                                where d.Id == tempId
        //                                select d).SingleOrDefault();
        //                    prodLs.Add(data);
        //                }
        //            }
        //            var config = new MapperConfiguration(cfg =>
        //            {
        //                cfg.CreateMap<Product, ProductDTO>();
        //            });
        //            var mapper = new Mapper(config);
        //            var cData = mapper.Map<List<ProductDTO>>(prodLs);

        //            return View(cData);
        //        }
        //        return RedirectToAction("Dashboard");
        //    }
        //    return RedirectToAction("Login");
        //}
        [Logged]
        public ActionResult RemoveFromCart(int Id)
        {
            if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
            {
                if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
                {
                    var pdDictionary = new Dictionary<int, int>();
                    pdDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));

                    if (pdDictionary.ContainsKey(Id))
                    {
                        pdDictionary[Id] -= 1; // Decrement the count for the product

                        if (pdDictionary[Id] <= 0)
                        {
                            pdDictionary.Remove(Id); // If the count is 0 or less, remove the product from the dictionary
                        }

                        var newCookie = new HttpCookie("pdList", HttpUtility.UrlEncode(JsonConvert.SerializeObject(pdDictionary)));
                        Response.Cookies.Add(newCookie);
                    }
                    TempData["removedFromCart"] = true;
                    return RedirectToAction("showCart");
                }
                TempData["noItemInCart"] = true;
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Login");
        }
        [Logged]
        [HttpPost]
        public ActionResult Logout()
        {
            Session["loggedIn"] = null;
            return RedirectToAction("Login");
        }
        //public ActionResult RemoveFromCart(int Id)
        //{
        //    if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
        //    {
        //        if (Request.Cookies["pdList"] != null && !string.IsNullOrEmpty(Request.Cookies["pdList"].Value))
        //        {
        //            var pdList = new List<int>();
        //            pdList = JsonConvert.DeserializeObject<List<int>>(HttpUtility.UrlDecode(Request.Cookies["pdList"].Value));
        //            if (pdList.Remove(Id))
        //            {
        //                var newCookie = new HttpCookie("pdList", HttpUtility.UrlEncode(JsonConvert.SerializeObject(pdList)));
        //                Response.Cookies.Add(newCookie);
        //            }
        //        }
        //        return RedirectToAction("showCart");
        //    }
        //    return RedirectToAction("Login");
        //}
        //public ActionResult RemoveFromCart(int Id)
        //{
        //    if (Request.Cookies["pdList"] != null)
        //    {
        //        var cookie = Request.Cookies["pdList"];
        //        var existingValue = cookie.Value;
        //        var newValue = existingValue.Replace(Id.ToString(),string.Empty);
        //        cookie.Value = newValue;
        //        Response.SetCookie(cookie);
        //    }

        //    return RedirectToAction("showCart");
        //}
        public ProductDTO convert(Product p)
        {
            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                CtId = p.CtId,
                Price = p.Price,
            };
        }
        public List<ProductDTO> convert(List<Product> p)
        {
            var pdList = new List<ProductDTO>();
            foreach (var item in p)
            {
                pdList.Add(convert(item));
            }
            return pdList;
        }
        public ActionResult GetProductsByCategory(int CtId)
        {
            var db = new InventoryMSEntities();
            var products = db.Products.Where(p => p.CtId == CtId).Select(p => new ProductDTO { Id = p.Id, Name = p.Name, Price = p.Price}).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}