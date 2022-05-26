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
    public class subController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: sub
        public ActionResult Index()
        {
            var subTheme = db.subTheme.Include(s => s.theme);
            return View(subTheme.ToList());
        }

        // GET: sub/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subTheme subTheme = db.subTheme.Find(id);
            if (subTheme == null)
            {
                return HttpNotFound();
            }
            return View(subTheme);
        }

        // GET: sub/Create
        public ActionResult Create()
        {
            ViewBag.themeID = new SelectList(db.theme, "ID", "name");
            return View();
        }

        // POST: sub/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,code,description,icons,themeID")] subTheme subTheme)
        {
            if (ModelState.IsValid)
            {
                db.subTheme.Add(subTheme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.themeID = new SelectList(db.theme, "ID", "name", subTheme.themeID);
            return View(subTheme);
        }

        // GET: sub/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subTheme subTheme = db.subTheme.Find(id);
            if (subTheme == null)
            {
                return HttpNotFound();
            }
            ViewBag.themeID = new SelectList(db.theme, "ID", "name", subTheme.themeID);
            return View(subTheme);
        }

        // POST: sub/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,code,description,icons,themeID")] subTheme subTheme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subTheme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.themeID = new SelectList(db.theme, "ID", "name", subTheme.themeID);
            return View(subTheme);
        }

        // GET: sub/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subTheme subTheme = db.subTheme.Find(id);
            if (subTheme == null)
            {
                return HttpNotFound();
            }
            return View(subTheme);
        }

        // POST: sub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subTheme subTheme = db.subTheme.Find(id);
            db.subTheme.Remove(subTheme);
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
