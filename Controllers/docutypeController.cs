using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using crown.Models;

namespace crown.Controllers
{
    public class docutypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: docutype
        public ActionResult Index()
        {
            return View(db.archiveType.ToList());
        }

        // GET: docutype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archiveType archiveType = db.archiveType.Find(id);
            if (archiveType == null)
            {
                return HttpNotFound();
            }
            return View(archiveType);
        }

        // GET: docutype/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: docutype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,description")] archiveType archiveType)
        {
            if (ModelState.IsValid)
            {
                db.archiveType.Add(archiveType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(archiveType);
        }

        // GET: docutype/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archiveType archiveType = db.archiveType.Find(id);
            if (archiveType == null)
            {
                return HttpNotFound();
            }
            return View(archiveType);
        }

        // POST: docutype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,description")] archiveType archiveType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archiveType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archiveType);
        }

        // GET: docutype/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archiveType archiveType = db.archiveType.Find(id);
            if (archiveType == null)
            {
                return HttpNotFound();
            }
            return View(archiveType);
        }

        // POST: docutype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            archiveType archiveType = db.archiveType.Find(id);
            db.archiveType.Remove(archiveType);
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
