using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolWebEFNimaV2.Models;
using System.Data.Entity.Infrastructure;

namespace SchoolWebEFNimaV2.Controllers
{
    public class CoursesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Courses
        public ActionResult Index()
        {
            // Here i included the teacher table into my courses table by the link that i created earlier in course model
            var Courses = db.Courses.Include(o => o.Teacher);
            // return the mixed table as a list to view
            return View(Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            // to handle null for ID
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // gere we create an isntance of our course model, we include teachers, students and assignments in it, adn connect all of them based on the courses that have the courseId equal to id,
            // the id gets to the method by the actionlink that we had infront of each row of courses
            Course course = db.Courses.Include(i => i.Teacher).Include(j => j.Students).Include(k => k.Assignments).SingleOrDefault(x => x.CourseID == id);
            // to handle null for Course
            if (course == null)
            {
                return HttpNotFound();
            }
            // returns the course to the view
            return View(course);
        }
        
        // GET: Home/Create
        public ActionResult Create()
        {
            //put the informatoin of the teachers table in viewbag which is called TeacherId, to prepare it for dropdownmenu
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherID", "TeacherName", "TeacherLastName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // gets the selected teacher and connected to the its related course, The teacherId is important to have here, in order to be 
        // able to fill it later in ViewBag.TeacherId
        public ActionResult Create([Bind(Include = "CourseID,CourseName,StartDate,EndDate,TeacherId")] Course course)
        {
            // if all variables are valid then the course will be added to the databse in course table and the changes will be save
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // the result of the dropdownmune will be presented here in course.TeacherId
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherID", "TeacherLastName", course.TeacherId);
            // returns the course with its new teacherId to view
            return View(course);
        }


        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            // provide a dropdown list, the same way as create method
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherID","TeacherName","TeacherLastName");
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,StartDate,EndDate,TeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherID", "TeacherLastName", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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

        [HttpGet]
        public ActionResult AddStudents(int? id) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "StudentLastName");
            //ViewBag.CourseId = new SelectList(db.Courses, "CourseID", "CourseName");
            return View(course);

        }


        // add student to the courses, the Student id will be selected from dropdown list and the course Id just sent in from the course row 
        [HttpPost]
        public ActionResult AddStudents(int StudentId, int CourseId)
        {
            // find the course and student base in their Ids
            Student student = db.Students.Find(StudentId);
            Course course = db.Courses.Find(CourseId);

            // add the student to the list of students in the course
            course.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
