using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using crown.Models;
using Microsoft.AspNet.Identity;
using SelectPdf;

namespace crown.Controllers
{
    public class archivesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: archives
        public ActionResult Index()
        {
            var archive = db.archive.Include(a => a.archiveType);
            return View(archive.ToList());
        }

        // GET: archives/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archive archive = db.archive.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // GET: archives/Create
        public ActionResult Create()
        {
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View();
        }

        // POST: archives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,description,archiveTypeID,fileName")] archive archive)
        {
            if (ModelState.IsValid)
            {
                db.archive.Add(archive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description", archive.archiveTypeID);
            return View(archive);
        }

        // GET: archives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archive archive = db.archive.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description", archive.archiveTypeID);
            return View(archive);
        }

        // POST: archives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,description,archiveTypeID,fileName")] archive archive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description", archive.archiveTypeID);
            return View(archive);
        }

        // GET: archives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            archive archive = db.archive.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: archives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            archive archive = db.archive.Find(id);
            db.archive.Remove(archive);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Reference(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Referensi dari Daftar Arsip";
            var timeline = db.subThemeItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.timelineItem.Title;
            var archives = db.archive.Where(y => y.archiveItem.Where(x => x.itemID == timeline.timelineItemID).Count() == 0).ToList();
            return View(archives);
        }

        public ActionResult Select(int id, int item)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            var timeline = db.subThemeItem.Find(item);
            var data = new archiveItem
            {
                archiveID = id,
                itemID = timeline.timelineItemID
            };
            db.archiveItem.Add(data);
            db.SaveChanges();
            return RedirectToAction("Details","themes",new { id=item });
        }

        public ActionResult AddReference(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Referensi dari Daftar Arsip";
            var timeline = db.timelineItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.Title;
            var archives = db.archive.Where(y => y.archiveItem.Where(x => x.itemID == timeline.ID).Count() == 0).ToList();
            return View(archives);
        }

        public ActionResult SelectDocument(int id, int item)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            var data = new archiveItem
            {
                archiveID = id,
                itemID = item
            };
            db.archiveItem.Add(data);
            db.SaveChanges();
            return RedirectToAction("Detail", "themes", new { id = item });
        }

        public ActionResult FromURL(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Dokumen dari URL";
            var timeline = db.subThemeItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.timelineItem.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View();
        }

        [HttpPost]
        public ActionResult FromURL([Bind(Include = "url,description,archiveTypeID,timelineItemID")] UrltoHtmlModel databind)
        {
            var timeline = db.subThemeItem.Find(databind.timelineItemID);
            var subtheme = timeline.subTheme;
            // read parameters from the webpage
            string url = databind.url;
            string page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), page_size, true);
            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);
            int webPageWidth = 1024;
            int webPageHeight = 0;
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            byte[] pdf = doc.Save();

            //// close pdf document
            //doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            var fileName = Regex.Replace(databind.description, "[^A-Za-z0-9 ]", "").Replace(' ','-').ToLower() + fileTimeStamp + ".pdf";
            var path = Path.Combine(Server.MapPath("~/Documents/"), fileName);
            fileResult.FileDownloadName = fileName;
            System.IO.File.WriteAllBytes(path, pdf);

            var data = new archive
            {
                archiveTypeID = databind.archiveTypeID,
                description = databind.description,
                fileName = fileName,
                origin = databind.url
            };
            db.archive.Add(data);
            db.SaveChanges();

            var data2 = new archiveItem
            {
                archiveID = data.ID,
                itemID = timeline.timelineItemID
            };

            db.archiveItem.Add(data2);
            db.SaveChanges();

            return RedirectToAction("Details","themes", new { id=timeline.ID });
        }

        public ActionResult AddFromURL(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Dokumen dari URL";
            var timeline = db.timelineItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View("FromURL");
        }

        [HttpPost]
        public ActionResult AddFromURL([Bind(Include = "url,description,archiveTypeID,timelineItemID")] UrltoHtmlModel databind)
        {
            var timeline = db.timelineItem.Find(databind.timelineItemID);
            // read parameters from the webpage
            string url = databind.url;
            string page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), page_size, true);
            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);
            int webPageWidth = 1024;
            int webPageHeight = 0;
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            // save pdf document
            byte[] pdf = doc.Save();

            //// close pdf document
            //doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            var fileName = Regex.Replace(databind.description, "[^A-Za-z0-9 ]", "").Replace(' ', '-').ToLower() + fileTimeStamp + ".pdf";
            var path = Path.Combine(Server.MapPath("~/Documents/"), fileName);
            fileResult.FileDownloadName = fileName;
            System.IO.File.WriteAllBytes(path, pdf);

            var data = new archive
            {
                archiveTypeID = databind.archiveTypeID,
                description = databind.description,
                fileName = fileName,
                origin = databind.url
            };
            db.archive.Add(data);
            db.SaveChanges();

            var data2 = new archiveItem
            {
                archiveID = data.ID,
                itemID = timeline.ID
            };

            db.archiveItem.Add(data2);
            db.SaveChanges();

            return RedirectToAction("Detail", "themes", new { id = timeline.ID });
        }

        public ActionResult Upload(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Upload File Dokumen Sumber";
            var timeline = db.subThemeItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.timelineItem.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Upload([Bind(Include = "path,description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        //{
        //    var timeline = db.subThemeItem.Find(model.timelineItemID);
        //    var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
        //    var fileName = Regex.Replace(model.description, "[^A-Za-z0-9 ]", "").Replace(' ', '-').ToLower() + fileTimeStamp + ".pdf";

        //    model.file.SaveAs(HttpContext.Server.MapPath("~/Documents/") + fileName);

        //    var data = new archive
        //    {
        //        archiveTypeID = model.archiveTypeID,
        //        description = model.description,
        //        fileName = fileName,
        //        origin = model.url
        //    };
        //    db.archive.Add(data);
        //    db.SaveChanges();

        //    var data2 = new archiveItem
        //    {
        //        archiveID = data.ID,
        //        itemID = timeline.timelineItemID
        //    };

        //    db.archiveItem.Add(data2);
        //    db.SaveChanges();

        //    return RedirectToAction("Details", "themes", new { id = timeline.ID });

        //}

        //[HttpPost, ActionName("Upload")]
        //[ValidateAntiForgeryTokesn]
        //public ActionResult UploadFile([Bind(Include="description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        //{

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "path,description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        {
            var timeline = db.subThemeItem.Find(model.timelineItemID);
            var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            if (model.path == "web")
            {
                var fileName = fileTimeStamp + model.file.FileName;
                var client = new System.Net.WebClient();
                client.DownloadFile(model.url, Server.MapPath("~/Documents/" + fileName));

                var data1 = new archive
                {
                    archiveTypeID = model.archiveTypeID,
                    description = model.description,
                    fileName = fileName,
                    origin = model.url
                };

                db.archive.Add(data1);
                db.SaveChanges();

                var data2 = new archiveItem
                {
                    itemID = timeline.timelineItemID,
                    archiveID = data1.ID
                };

                db.archiveItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Details", "themes", new { id = timeline.timelineItemID });
            }
            else if (model.path == "file")
            {
                //Use Namespace called :  System.IO  
                string FileNameOnly = Path.GetFileNameWithoutExtension(model.file.FileName);

                //To Get File Extension  
                string FileExtension = Path.GetExtension(model.file.FileName);

                var fileName = Regex.Replace(model.description, "[^A-Za-z0-9 ]", "").Replace(' ', '-').ToLower() + fileTimeStamp + FileExtension;
                //Add Current Date To Attached File Name  

                //Get Upload path from Web.Config file AppSettings.  
                string UploadPath = HttpContext.Server.MapPath("~/Documents/");

                //Its Create complete path to store in server.  
                string resultPath = UploadPath + fileName;

                //To copy and save file into server.  
                model.file.SaveAs(resultPath);

                var data = new archive
                {
                    archiveTypeID = model.archiveTypeID,
                    description = model.description,
                    fileName = fileName,
                    origin = model.url
                };
                db.archive.Add(data);
                db.SaveChanges();

                var data2 = new archiveItem
                {
                    archiveID = data.ID,
                    itemID = timeline.timelineItemID
                };

                db.archiveItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Details", "themes", new { id = timeline.ID });

                db.archiveItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Details", "themes", new { id = timeline.timelineItemID });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Uploads(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Upload File Dokumen Sumber";
            var timeline = db.timelineItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Uploads([Bind(Include = "path,description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        //{
        //    var timeline = db.timelineItem.Find(model.timelineItemID);

        //    //Use Namespace called :  System.IO  
        //    string FileNameOnly = Path.GetFileNameWithoutExtension(model.file.FileName);

        //    //To Get File Extension  
        //    string FileExtension = Path.GetExtension(model.file.FileName);

        //    var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");

        //    var fileName = Regex.Replace(model.description, "[^A-Za-z0-9 ]", "").Replace(' ', '-').ToLower() + fileTimeStamp + FileExtension;
        //    //Add Current Date To Attached File Name  

        //    //Get Upload path from Web.Config file AppSettings.  
        //    string UploadPath = HttpContext.Server.MapPath("~/Documents/");

        //    //Its Create complete path to store in server.  
        //    string resultPath = UploadPath + fileName;

        //    //To copy and save file into server.  
        //    model.file.SaveAs(resultPath);

        //    var data = new archive
        //    {
        //        archiveTypeID = model.archiveTypeID,
        //        description = model.description,
        //        fileName = fileName,
        //        origin = model.url
        //    };
        //    db.archive.Add(data);
        //    db.SaveChanges();

        //    var data2 = new archiveItem
        //    {
        //        archiveID = data.ID,
        //        itemID = timeline.ID
        //    };

        //    db.archiveItem.Add(data2);
        //    db.SaveChanges();

        //    return RedirectToAction("Detail", "themes", new { id = timeline.ID });
        //}

        //[HttpPost, ActionName("Uploads")]
        //[ValidateAntiForgeryToken]
        //public ActionResult UploadFiles([Bind(Include = "description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        //{

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Uploads([Bind(Include = "path,description,archiveTypeID,timelineItemID,url,file")] UploadFileModel model)
        {
            var timeline = db.timelineItem.Find(model.timelineItemID);
            var fileTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");

            if (model.path == "web")
            {
                var fileName = fileTimeStamp + model.file.FileName;
                var client = new System.Net.WebClient();
                client.DownloadFile(model.url, Server.MapPath("~/Documents/" + fileName));

                var data1 = new archive
                {
                    archiveTypeID = model.archiveTypeID,
                    description = model.description,
                    fileName = fileName,
                    origin = model.url
                };

                db.archive.Add(data1);
                db.SaveChanges();

                var data2 = new archiveItem
                {
                    itemID = timeline.ID,
                    archiveID = data1.ID
                };

                db.archiveItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Detail", "themes", new { id = timeline.ID });
            }
            else if (model.path == "file")
            {

                //Use Namespace called :  System.IO  
                string FileNameOnly = Path.GetFileNameWithoutExtension(model.file.FileName);

                //To Get File Extension  
                string FileExtension = Path.GetExtension(model.file.FileName);

                var fileName = Regex.Replace(model.description, "[^A-Za-z0-9 ]", "").Replace(' ', '-').ToLower() + fileTimeStamp + FileExtension;
                //Add Current Date To Attached File Name  

                //Get Upload path from Web.Config file AppSettings.  
                string UploadPath = HttpContext.Server.MapPath("~/Documents/");

                //Its Create complete path to store in server.  
                string resultPath = UploadPath + fileName;

                //To copy and save file into server.  
                model.file.SaveAs(resultPath);

                var data = new archive
                {
                    archiveTypeID = model.archiveTypeID,
                    description = model.description,
                    fileName = fileName,
                    origin = model.url
                };
                db.archive.Add(data);
                db.SaveChanges();

                var data2 = new archiveItem
                {
                    archiveID = data.ID,
                    itemID = timeline.ID
                };

                db.archiveItem.Add(data2);
                db.SaveChanges();

                return RedirectToAction("Detail", "themes", new { id = timeline.ID });
            }

            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult FromOnlineRepo(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Dokumen dari Repositori Online";
            var timeline = db.subThemeItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.timelineItem.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View();
        }

        [HttpPost]
        public ActionResult FromOnlineRepo([Bind(Include = "url,description,archiveTypeID,timelineItemID")] UrltoHtmlModel databind)
        {
            var timeline = db.subThemeItem.Find(databind.timelineItemID);
            var subtheme = timeline.subTheme;
            var data = new archive
            {
                archiveTypeID = databind.archiveTypeID,
                description = databind.description,
                origin = databind.url,
                savedInOnlineRepository = true
            };
            db.archive.Add(data);
            db.SaveChanges();

            var data2 = new archiveItem
            {
                archiveID = data.ID,
                itemID = timeline.timelineItemID
            };

            db.archiveItem.Add(data2);
            db.SaveChanges();

            return RedirectToAction("Details", "themes", new { id = timeline.ID });
        }

        public ActionResult AddFromOnlineRepo(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser.isGuest)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Tambah Dokumen dari Repositori Online";
            var timeline = db.timelineItem.Find(id);
            ViewBag.timelineID = id;
            ViewBag.desc = timeline.Title;
            ViewBag.archiveTypeID = new SelectList(db.archiveType, "ID", "description");
            return View("FromOnlineRepo");
        }

        [HttpPost]
        public ActionResult AddFromOnlineRepo([Bind(Include = "url,description,archiveTypeID,timelineItemID")] UrltoHtmlModel databind)
        {
            var timeline = db.timelineItem.Find(databind.timelineItemID);
            var data = new archive
            {
                archiveTypeID = databind.archiveTypeID,
                description = databind.description,
                origin = databind.url,
                savedInOnlineRepository = true
            };
            db.archive.Add(data);
            db.SaveChanges();

            var data2 = new archiveItem
            {
                archiveID = data.ID,
                itemID = timeline.ID
            };

            db.archiveItem.Add(data2);
            db.SaveChanges();

            return RedirectToAction("Detail", "themes", new { id = timeline.ID });
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
