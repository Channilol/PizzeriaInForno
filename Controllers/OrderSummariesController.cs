﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzeriaInForno.Models;

namespace PizzeriaInForno.Controllers
{
    public class OrderSummariesController : Controller
    {
        private DBContext db = new DBContext();

        [Authorize]
        public ActionResult Summary()
        {

            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            OrderSummary summId = db.OrderSummaries.Where(o => o.UserId == userId && o.State == "NON EVASO").FirstOrDefault();
            OrderSummary summary = db.OrderSummaries
                                    .Include(o => o.OrderItems)
                                    .Include(o => o.OrderItems.Select(i => i.Product))
                                    .SingleOrDefault(o => o.OrderSummaryId == summId.OrderSummaryId);
            if (summary != null)
            {
                ViewBag.SumPrice = summary.OrderItems.Sum(i => i.ItemPrice);
                return View(summary);
            }
            else
            {
                return View();
            }
        }


        // GET: OrderSummaries
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var orderSummaries = db.OrderSummaries.Include(o => o.User);
            return View(orderSummaries.ToList());
        }

        // GET: OrderSummaries/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            return View(orderSummary);
        }

        // GET: OrderSummaries/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: OrderSummaries/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderSummaryId,UserId,OrderDate,OrderAddress,Note,TotalPrice,State")] OrderSummary orderSummary)
        {
            if (ModelState.IsValid)
            {
                db.OrderSummaries.Add(orderSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // GET: OrderSummaries/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrderSummary orderSummary = db.OrderSummaries.Find(id);

            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // POST: OrderSummaries/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderAddress,Note,OrderDate")] OrderSummary orderSummary, int id)
        {
            if (id < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrderSummary orderToSend = db.OrderSummaries.Find(id);

            if (orderToSend == null)
            {
                return HttpNotFound();
            }

            decimal sumPrice = db.OrderItems.Where(o => o.OrderSummaryId == id).Sum(i => i.ItemPrice);

            if (ModelState.IsValid)
            {
                orderToSend.OrderDate = orderSummary.OrderDate;
                orderToSend.TotalPrice = sumPrice;
                orderToSend.State = "EVASO";
                orderToSend.OrderAddress = orderSummary.OrderAddress;
                orderToSend.Note = orderSummary.Note;

                db.Entry(orderToSend).State = EntityState.Modified;
                db.SaveChanges();

                TempData["OrderSuccess"] = true;

                return RedirectToAction("Index", "Home");
            }

            return View(orderToSend);
        }

        // GET: OrderSummaries/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            return View(orderSummary);
        }

        // POST: OrderSummaries/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            db.OrderSummaries.Remove(orderSummary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            OrderItem item = db.OrderItems.Find(id);
            if (item != null)
            {
                db.OrderItems.Remove(item);
                db.SaveChanges();
                TempData["ItemRemoved"] = true;
                return RedirectToAction("Summary");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")] 
        public ActionResult OrdiniEvasi()
        {
            var ordiniEvasi = db.OrderSummaries.Where(o => o.State == "EVASO").Count();
            return Json(ordiniEvasi, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IncassiPerGiorno(int id)
        {
            var ordiniTotali = db.OrderSummaries.Where(o => o.State == "EVASO").ToList();
            decimal incassoPerGiorno = 0;
            foreach (var order in ordiniTotali)
            {
                string dateString = order.OrderDate.ToString();
                string[] dataArray = dateString.Split('/');
                int day = Convert.ToInt32(dataArray[0]);
                if (day == id)
                {
                    incassoPerGiorno += order.TotalPrice;
                }
            }
            
            return Json(incassoPerGiorno, JsonRequestBehavior.AllowGet);
        }
    }
}
