﻿using System;
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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace ChieftensLMS.Controllers
{
    public class CourseController : Controller
    {
		private LMSDbContext _context;
		private CourseService _courseService;

		public CourseController()
		{
			_context = new LMSDbContext();
			_courseService = new CourseService(_context);
		}

        // GET: Courses
        public ActionResult Index()
		{ 
			return View();
        }

		// GET: Courses/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var course = _courseService.GetCourseById((int)id);

			if (course == null)
			{
				return HttpNotFound();
			}
			return View(course);
		}

		//// GET: Courses/Create
		//public ActionResult Create()
		//{
		//    return View();
		//}

		//// POST: Courses/Create
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create([Bind(Include = "Id,Name")] Course course)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Courses.Add(course);
		//        db.SaveChanges();
		//        return RedirectToAction("Index");
		//    }

		//    return View(course);
		//}

		//// GET: Courses/Edit/5
		//public ActionResult Edit(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Course course = db.Courses.Find(id);
		//    if (course == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(course);
		//}

		//// POST: Courses/Edit/5
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Edit([Bind(Include = "Id,Name")] Course course)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Entry(course).State = EntityState.Modified;
		//        db.SaveChanges();
		//        return RedirectToAction("Index");
		//    }
		//    return View(course);
		//}

		//// GET: Courses/Delete/5
		//public ActionResult Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Course course = db.Courses.Find(id);
		//    if (course == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(course);
		//}

		//// POST: Courses/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(int id)
		//{
		//    Course course = db.Courses.Find(id);
		//    db.Courses.Remove(course);
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
