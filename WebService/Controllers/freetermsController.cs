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
    public class freetermsController : Controller
    {
        private healthCenterDBEntities db = new healthCenterDBEntities();

        // GET: freeterms
        public ActionResult Index()
        {
            return View(db.freeterms.ToList());
        }

        // GET: freeterms/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freeterms freeterms = db.freeterms.Find(id);
            if (freeterms == null)
            {
                return HttpNotFound();
            }
            return View(freeterms);
        }

        // GET: freeterms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: freeterms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,doctorusername,date,doctorspeciality")] freeterms freeterms)
        {
            if (ModelState.IsValid)
            {
                freeterms.id = Guid.NewGuid();
                db.freeterms.Add(freeterms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(freeterms);
        }

        // GET: freeterms/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freeterms freeterms = db.freeterms.Find(id);
            if (freeterms == null)
            {
                return HttpNotFound();
            }
            return View(freeterms);
        }

        // POST: freeterms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,doctorusername,date,doctorspeciality")] freeterms freeterms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freeterms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(freeterms);
        }

        // GET: freeterms/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freeterms freeterms = db.freeterms.Find(id);
            if (freeterms == null)
            {
                return HttpNotFound();
            }
            return View(freeterms);
        }

        // POST: freeterms/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            freeterms freeterms = db.freeterms.Find(id);
            db.freeterms.Remove(freeterms);
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
