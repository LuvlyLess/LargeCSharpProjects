using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CarInsurance.Models;
using Microsoft.Ajax.Utilities;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();
        //Post: Quote
       

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            var DOB = insuree.DateOfBirth;  // creating a instance of the Date of birth year
            var currentDate = DateTime.Now; // creating an instance of current year to compare
            int age = currentDate.DayOfYear - DOB.DayOfYear; // finding the age of user

            decimal quoteRate = 50;
            
            
            if (age <= 18) // using an if statement to calulate age and ageRate
            {
                quoteRate += 100;
            }
            else if (age == 19 || age <= 25)
            {
                quoteRate += 50;
            }
            else if (age >= 26)
            {
                quoteRate += 25;
            }
            
            var carMake = insuree.CarMake.ToLower();
            var carModel = insuree.CarModel.ToLower();           
            insuree.CarMake.ToLower();

            var modelRateP = 25;
            var modelRate911 = 50; 
            if (carMake == "porsche".ToLower())
            {
                quoteRate += modelRateP;
            }
            else if (carMake == "porsche".ToLower() && carModel == "911 Carrera".ToLower())
            {
                quoteRate += modelRate911;
            }


            var monthlyRate = 10;
            int tickets = insuree.SpeedingTickets;
            foreach (int ticket in Convert.ToString(tickets))
            {
                quoteRate += monthlyRate;
            }

            if (insuree.DUI == true)
            {
                quoteRate *= 1.25m;
            }

            if (insuree.CoverageType == true)
            {
                quoteRate *= 1.50m;
            }
            insuree.Quote = quoteRate;
            
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
            }
             
            return View(insuree);
            
            
        }

        public ActionResult AdminView()
        {

            return View(db.Insurees.ToList());
        }














        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
