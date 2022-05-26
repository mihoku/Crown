using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using crown.Models;
using Microsoft.AspNet.Identity;

namespace crown.Controllers
{
    public class themesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: themes
        //public ActionResult Index()
        //{
        //    return View(db.theme.ToList());
        //}

        public ActionResult data(string id)
        {
            ViewBag.code = id;
            return PartialView();
        }

        public ActionResult monitoring()
        {
            var generalpolicy = db.timelineItem.Where(y=>y.isGeneral).ToList();
            var generaldocument = db.archive.Where(y => y.archiveItem.Where(x => x.timelineItem.isGeneral).Count() != 0).ToList();
            ViewBag.infoCount = generalpolicy.Count();
            ViewBag.documentCount = generaldocument.Count();
            var data = db.theme.ToList();
            return View(data);
        }
        public ActionResult documentCount(int id)
        {
            var themedocument = db.archive.Where(y => y.archiveItem.Where(x => x.timelineItem.subThemeItem.Where(t=>t.subThemeID==id).Count()!=0).Count() != 0).ToList();
            var data = themedocument.Count().ToString();
            return Content(data);
        }
        public ActionResult Amend(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            var item = db.timelineItem.Find(id);
            ViewBag.themeID = id;
            ViewBag.Contents = item.Contents;
            var data = new AddTimelineModel
            {
                subthemeID = id,
                EventDate = item.EventDate,
                Title = item.Title,
                Contents = item.Contents
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Amend([Bind(Include = "subthemeID,EventDate,Title,Contents")] AddTimelineModel data)
        {
            var item = db.timelineItem.Find(data.subthemeID);
            item.EventDate = data.EventDate;
            item.Title = data.Title;
            item.Contents = data.Contents;
            db.SaveChanges();
            if (item.subThemeItem.Count() != 0)
            {
                var firsttheme = item.subThemeItem.First();
                return RedirectToAction("Details", new { id = firsttheme.ID });
            }
            else
            {
                return RedirectToAction("Detail", new { id = item.ID });
            }
        }

        // GET: themes/Details/5
        public ActionResult Details(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ViewBag.isAdmin = !currentUser.isGuest;
            subThemeItem info = db.subThemeItem.Find(id);
            ViewBag.Title = info.subTheme.name;
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        public ActionResult Detail(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ViewBag.isAdmin = !currentUser.isGuest;
            timelineItem info = db.timelineItem.Find(id);
            ViewBag.Title = "Dokumentasi Kebijakan Pemerintah dalam Penanganan Pandemi COVID-19";
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        //create from informasi umum
        public ActionResult Creates(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            timelineItem item = db.timelineItem.Find(id);
            ViewBag.itemID = item.ID;
            ViewBag.Title = "Tambah Informasi Detail";
            ViewBag.Contents = "";
            return View();
        }

        // POST: themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creates([Bind(Include = "itemID,Title,Contents")] timelineDetailsAddModel details)
        {
            if (ModelState.IsValid)
            {
                var data1 = new timelineDetails
                {
                    Contents = details.Contents,
                    Title = details.Title,
                    itemID = details.itemID
                };

                db.timelineDetails.Add(data1);
                db.SaveChanges();

                return RedirectToAction("Detail", new { id = details.itemID });
            }

            ViewBag.Title = "Tambah Informasi Detail";
            ViewBag.Contents = details.Contents;
            ViewBag.itemID = details.itemID;
            return View(details);
        }

        //create from subtheme
        public ActionResult Create(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            subThemeItem item = db.subThemeItem.Find(id);
            ViewBag.itemID = item.ID;
            ViewBag.Title = "Tambah Informasi Detail";
            ViewBag.Contents = "";
            return View("Creates");
        }

        // POST: themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "itemID,Title,Contents")] timelineDetailsAddModel details)
        {
            if (ModelState.IsValid)
            {
                var subthemeItem = db.subThemeItem.Find(details.itemID);
                var data1 = new timelineDetails
                {
                    Contents = details.Contents,
                    Title = details.Title,
                    itemID = subthemeItem.timelineItemID
                };

                db.timelineDetails.Add(data1);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = details.itemID });
            }

            ViewBag.Title = "Tambah Informasi Detail";
            ViewBag.Contents = details.Contents;
            ViewBag.itemID = details.itemID;
            return View("Creates", details);
        }

        public ActionResult AddInfo(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            subTheme theme = db.subTheme.Find(id);
            ViewBag.themeID = id;
            ViewBag.Title = "Tambah Informasi untuk " +theme.name;
            ViewBag.Contents = "";
            return View();
        }

        // POST: themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInfo([Bind(Include = "subthemeID,Title,Contents,EventDate")] AddTimelineModel theme)
        {
            if (ModelState.IsValid)
            {
                var data1 = new timelineItem
                {
                    EventDate = theme.EventDate,
                    Contents = theme.Contents,
                    Title = theme.Title
                };

                db.timelineItem.Add(data1);
                db.SaveChanges();

                var data2 = new subThemeItem
                {
                    timelineItemID = data1.ID,
                    subThemeID = theme.subthemeID
                };

                db.subThemeItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Display",new { id=theme.subthemeID });
            }

            var subtheme = db.subTheme.Find(theme.subthemeID);
            ViewBag.themeID = theme.subthemeID;
            ViewBag.Title = "Tambah Informasi untuk " + subtheme.name;
            ViewBag.Contents = theme.Contents;
            return View(theme);
        }

        public ActionResult InfoAdd()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Informasi Umum Kebijakan Pemerintah";
            ViewBag.Contents = "";
            return View();
        }

        // POST: themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InfoAdd([Bind(Include = "Title,Contents,EventDate")] AddTimelineModel theme)
        {
            if (ModelState.IsValid)
            {
                var data1 = new timelineItem
                {
                    EventDate = theme.EventDate,
                    Contents = theme.Contents,
                    Title = theme.Title,
                    isGeneral = true
                };

                db.timelineItem.Add(data1);
                db.SaveChanges();

                return RedirectToAction("Index", "Home", null);
            }

            ViewBag.Title = "Tambah Informasi Umum Kebijakan Pemerintah";
            ViewBag.Contents = theme.Contents;
            return View(theme);
        }

        public ActionResult source(int id)
        {
            var timeline = db.timelineItem.Find(id);
            ViewBag.Title = "Tambah Dokumen Sumber Pendukung";
            ViewBag.themeID = timeline.ID;
            return View();
        }

        public ActionResult sources(int id)
        {
            var timeline = db.subThemeItem.Find(id);
            ViewBag.Title = "Tambah Dokumen Sumber Pendukung";
            ViewBag.themeID = timeline.ID;
            return View();
        }

        // GET: themes/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: themes/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,name")] theme theme)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.theme.Add(theme);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(theme);
        //}

        // GET: themes/Edit/5
        public ActionResult Edit(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            subTheme theme = db.subTheme.Find(id);
            ViewBag.Title = "Edit Informasi Umum " + theme.name;
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // POST: themes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,description")] subTheme theme)
        {
            if (ModelState.IsValid)
            {
                var target = db.subTheme.Find(theme.ID);
                target.description = theme.description;
                db.SaveChanges();
                return RedirectToAction("Display",new { id=target.ID });
            }
            return View(theme);
        }

        //// GET: themes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    theme theme = db.theme.Find(id);
        //    if (theme == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(theme);
        //}

        //// POST: themes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    theme theme = db.theme.Find(id);
        //    db.theme.Remove(theme);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Sidebar()
        {
            var themes = db.theme.ToList();
            return PartialView(themes);
        }

        public ActionResult Display(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            var theme = db.subTheme.Find(id);
            ViewBag.Title = theme.name;
            ViewBag.isAdmin = !currentUser.isGuest;
            return View(theme);
        }

        public ActionResult spillCategory(int id)
        {
            ViewBag.itemID = id;
            var data = db.subTheme.ToList();
            return PartialView(data);
        }

        public ActionResult DeleteCategory(int id)
        {
            var data = db.subThemeItem.Find(id);
            var redirect = data.timelineItemID;
            db.subThemeItem.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Detail", new { id=redirect });
        }

        public ActionResult AddCategory(int id, int timeline)
        {
            var data = new subThemeItem
            {
                timelineItemID = timeline,
                subThemeID = id
            };
            db.subThemeItem.Add(data);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = data.ID });
        }

        public ActionResult SetGeneral(int id)
        {
            var data = db.timelineItem.Find(id);
            data.isGeneral = true;
            db.SaveChanges();
            return RedirectToAction("Detail", new { id = data.ID });
        }

        public ActionResult RemoveGeneral(int id)
        {
            var data = db.timelineItem.Find(id);
            data.isGeneral = false;
            db.SaveChanges();
            return RedirectToAction("Detail", new { id = data.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(searchModel keyword)
        {
            var data = db.timelineItem.Where(y => y.Title.Contains(keyword.terms)||y.Contents.Contains(keyword.terms)||y.timelineDetails.Where(z => z.Contents.Contains(keyword.terms) || z.Title.Contains(keyword.terms)).Count()!=0).ToList();
            ViewBag.Title = "Hasil Pencarian untuk '"+keyword.terms+"'";
            return View(data);
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
