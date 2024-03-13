using PizzeriaInForno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzeriaInForno.Controllers
{
    public class HomeController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.Identity.Name;

                var pastOrder = db.OrderSummaries.Where(o => o.UserId.ToString() == userId && o.State == "NON EVASO");

                if (pastOrder.Any())
                {
                }
                else
                {
                    OrderSummary newSummary = new OrderSummary();
                    newSummary.UserId = Convert.ToInt32(userId);
                    newSummary.State = "NON EVASO";
                    db.OrderSummaries.Add(newSummary);
                    db.SaveChanges();
                }
            }

            return View(db.Products.ToList());
        }
    }
}
