using MVCClaseSeis.Models;
using MVCClaseSeis.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Web;
using System.Web.Mvc;

namespace MVCClaseSeis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Service()
        {
            User user = new User();
            user.Name = "Elias";
            user.Age = 34;

            Product product = new Product();
            product.ProductName = "Phone";
            product.Price = 200;

            DashboardDto dto = new DashboardDto();
            dto.UserModel = user;
            dto.ProductModel = product;

            return View(dto);
        }
    }
}