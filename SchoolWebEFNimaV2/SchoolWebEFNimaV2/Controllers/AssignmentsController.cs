using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolWebEFNimaV2.Models;

namespace SchoolWebEFNimaV2.Controllers
{
    public class AssignmentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Assignments
        public ActionResult Index()
        {
            var Assignments = db.Assignments.Include(o => o.RelatedCourse).Include(o => o.RelatedTeacher);
            return View(Assignments.ToList());
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Assignment assignment = db.Assignments.Find(id);

            Assignment assignment = db.Assignments.Include(i => i.RelatedCourse).Include(j => j.RelatedTeacher).SingleOrDefault(x => x.AssignmentID == id);

            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseID", "CourseName", "StartDate", "EndDate", "TeacherId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentID,AssignmentName,UploadDate,DeadlineDate,CourseId,TeacherId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                // I wanted to get access to my teacherId and courseId and from them to the course name and teacherName, since while creating assignment, user decides
                // which course it should be assign to, it was no problem to get the courseId, but my teacherId was always null, in order to get access to my teacherId 
                // i had to go to course table and get the id from there, the below lines to the job
                assignment.TeacherId = db.Courses.SingleOrDefault(c => c.CourseID == assignment.CourseId).TeacherId;
                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseID", "CourseName", assignment.CourseId);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseID", "CourseName", "StartDate", "EndDate", "TeacherId");
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignmentID,AssignmentName,UploadDate,DeadlineDate,CourseId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseID", "CourseName", assignment.CourseId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
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
