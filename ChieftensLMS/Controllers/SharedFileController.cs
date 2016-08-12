using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;

namespace ChieftensLMS.Controllers
{
    public class SharedFileController : Controller
    {
		private ApplicationDbContext _context;
		private SharedFileService _sharedFileService;

		public SharedFileController()
		{
			_context = new ApplicationDbContext();
			_sharedFileService = new SharedFileService(_context);
		}

		// GET: SharedFiles
		public ActionResult Index(int? courseId)
        {
			if (courseId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var sharedFiles = _sharedFileService.GetSharedFilesForCourseId((int)courseId);

			if (sharedFiles == null)
			{
				return HttpNotFound();
			}
			
			return View(sharedFiles);
        }

        //// GET: SharedFiles/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SharedFile sharedFile = db.SharedFiles.Find(id);
        //    if (sharedFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sharedFile);
        //}

        //// GET: SharedFiles/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email");
        //    return View();
        //}

        //// POST: SharedFiles/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Name,Date,FileName,UserId,CourseId")] SharedFile sharedFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SharedFiles.Add(sharedFile);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", sharedFile.CourseId);
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", sharedFile.UserId);
        //    return View(sharedFile);
        //}

        //// GET: SharedFiles/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SharedFile sharedFile = db.SharedFiles.Find(id);
        //    if (sharedFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", sharedFile.CourseId);
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", sharedFile.UserId);
        //    return View(sharedFile);
        //}

        //// POST: SharedFiles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,Date,FileName,UserId,CourseId")] SharedFile sharedFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sharedFile).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", sharedFile.CourseId);
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", sharedFile.UserId);
        //    return View(sharedFile);
        //}

        //// GET: SharedFiles/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SharedFile sharedFile = db.SharedFiles.Find(id);
        //    if (sharedFile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sharedFile);
        //}

        //// POST: SharedFiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SharedFile sharedFile = db.SharedFiles.Find(id);
        //    db.SharedFiles.Remove(sharedFile);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
