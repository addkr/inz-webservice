using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebService.Models;

namespace WebService.Controllers
{
    public class specialitiesController : Controller
    {
        private healthCenterDBEntities1 db = new healthCenterDBEntities1();

       
        // GET: specialities
        public ActionResult Index()
        {
            return View(db.specialities.ToList());
        }

        // GET: specialities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialities specialities = db.specialities.Find(id);
            if (specialities == null)
            {
                return HttpNotFound();
            }
            return View(specialities);
        }

        // GET: specialities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: specialities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,name")] specialities specialities)
        {
            if (ModelState.IsValid)
            {
                specialities.id = Guid.NewGuid();
                db.specialities.Add(specialities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialities);
        }

        // GET: specialities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialities specialities = db.specialities.Find(id);
            if (specialities == null)
            {
                return HttpNotFound();
            }
            return View(specialities);
        }

        // POST: specialities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,name")] specialities specialities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialities);
        }

        // GET: specialities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialities specialities = db.specialities.Find(id);
            if (specialities == null)
            {
                return HttpNotFound();
            }
            return View(specialities);
        }

        // POST: specialities/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            specialities specialities = db.specialities.Find(id);
            db.specialities.Remove(specialities);
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
